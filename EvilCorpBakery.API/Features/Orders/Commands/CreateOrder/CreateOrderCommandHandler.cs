using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Data.Entities;
using EvilCorpBakery.API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Features.Orders.Command.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResponse>
    {
        private readonly EvilCorpBakeryAppDbContext _context;

        public CreateOrderCommandHandler(EvilCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        async Task<ApiResponse> IRequestHandler<CreateOrderCommand, ApiResponse>.Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            if(request == null || request.cartItems.Count() == 0)
            {
                return new ApiResponse
                {
                    Success = false,
                    Status = 400,
                    Title = "Invalid Order",
                    Detail = "Order must contain at least one item",
                    Type = "ValidationError"
                };
            }

            // Pobierz produkty z bazy danych
            var productIds = request.cartItems.Select(item => item.Id).ToList();
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, cancellationToken);

            // Sprawdź czy wszystkie produkty istnieją
            var missingProducts = productIds.Where(id => !products.ContainsKey(id)).ToList();
            if (missingProducts.Any())
            {
                return new ApiResponse
                {
                    Success = false,
                    Status = 404,
                    Title = "Product Not Found",
                    Detail = $"Products with IDs {string.Join(", ", missingProducts)} were not found",
                    Type = "NotFoundError"
                };
            }

            // Sprawdź dostępność stanów magazynowych
            var insufficientStockItems = new List<string>();
            foreach (var item in request.cartItems)
            {
                var product = products[item.Id];
                if (product.Stock < item.Quantity)
                {
                    insufficientStockItems.Add($"{product.Name} (available: {product.Stock}, requested: {item.Quantity})");
                }
            }

            if (insufficientStockItems.Any())
            {
                return new ApiResponse
                {
                    Success = false,
                    Status = 409,
                    Title = "Insufficient Stock",
                    Detail = $"Insufficient stock for: {string.Join("; ", insufficientStockItems)}",
                    Type = "StockError"
                };
            }

            // Rozpocznij transakcję
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                // Zmniejsz stany magazynowe
                foreach (var item in request.cartItems)
                {
                    var product = products[item.Id];
                    product.Stock -= item.Quantity;
                    product.UpdatedAt = DateTime.UtcNow;
                }

                // Wygeneruj OrderGuid
                var currentYear = DateTime.UtcNow.Year;
                var lastOrder = await _context.Orders
                    .Where(o => o.CreatedAt.Year == currentYear)
                    .OrderByDescending(o => o.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                int nextNumber = 1;
                if (lastOrder != null && !string.IsNullOrEmpty(lastOrder.OrderGuid))
                {
                    var parts = lastOrder.OrderGuid.Split('-');
                    if (parts.Length == 3 && int.TryParse(parts[2], out int lastNumber))
                    {
                        nextNumber = lastNumber + 1;
                    }
                }

                string orderGuid = $"#ORD-{currentYear}-{nextNumber:D3}";

                // Utwórz zamówienie
                Order order = new Order
                {
                    UserId = request.userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    TotalAmount = request.total,
                    StatusId = 1,
                    OrderItems = request.cartItems.Select(item => new OrderItem
                    {
                        ProductId = item.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.Price
                    }).ToList(),
                    AddressId = request.selectedAddressDto.Id,
                    Notes = request.notes,
                    PaymentMethod = request.PaymentMethodId,
                    OrderGuid = orderGuid,
                };

                await _context.Orders.AddAsync(order, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return new ApiResponse
                {
                    Success = true,
                    Status = 201,
                    Title = "Order Created",
                    Detail = $"Order {orderGuid} has been created successfully",
                    Type = "Success"
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new ApiResponse
                {
                    Success = false,
                    Status = 500,
                    Title = "Order Creation Failed",
                    Detail = $"An error occurred while creating the order: {ex.Message}",
                    Type = "ServerError"
                };
            }
        }
    }
}
