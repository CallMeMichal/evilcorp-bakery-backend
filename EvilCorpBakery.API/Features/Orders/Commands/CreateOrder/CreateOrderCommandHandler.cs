using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Data.Entities;
using EvilCorpBakery.API.Features.Orders.Command.CreateOrder;
using EvilCorpBakery.API.Middleware.Exceptions;
using EvilCorpBakery.API.Models.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderCreatedDto>
{
    private readonly EvilCorpBakeryAppDbContext _context;

    public CreateOrderCommandHandler(EvilCorpBakeryAppDbContext context)
    {
        _context = context;
    }

    async Task<OrderCreatedDto> IRequestHandler<CreateOrderCommand, OrderCreatedDto>.Handle(
        CreateOrderCommand request, CancellationToken cancellationToken)
    {
        if (request == null || request.cartItems.Count() == 0)
        {
            throw new BadRequestException("Order must contain at least one item");
        }

        var productIds = request.cartItems.Select(item => item.Id).ToList();
        var products = await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, cancellationToken);

        var missingProducts = productIds.Where(id => !products.ContainsKey(id)).ToList();
        if (missingProducts.Any())
        {
            throw new NotFoundException(
                $"Products with IDs {string.Join(", ", missingProducts)} were not found");
        }

        var insufficientStockItems = new List<string>();
        foreach (var item in request.cartItems)
        {
            var product = products[item.Id];
            if (product.Stock < item.Quantity)
            {
                insufficientStockItems.Add(
                    $"{product.Name} (available: {product.Stock}, requested: {item.Quantity})");
            }
        }

        if (insufficientStockItems.Any())
        {
            throw new ConflictException(
                $"Insufficient stock for: {string.Join("; ", insufficientStockItems)}");
        }

        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            foreach (var item in request.cartItems)
            {
                var product = products[item.Id];
                product.Stock -= item.Quantity;
                product.UpdatedAt = DateTime.UtcNow;
            }

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
                AddressId = request.selectedAddressDto?.Id,
                Notes = request.notes,
                PaymentMethod = request.PaymentMethodId,
                OrderGuid = orderGuid,
            };

            await _context.Orders.AddAsync(order, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new OrderCreatedDto
            {
                OrderGuid = orderGuid,
                OrderId = order.Id
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}