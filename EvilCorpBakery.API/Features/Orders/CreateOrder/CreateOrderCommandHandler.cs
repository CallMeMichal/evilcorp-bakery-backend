using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Features.Orders.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        private readonly EvilCorpBakeryAppDbContext _context;

        public CreateOrderCommandHandler(EvilCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        async Task<bool> IRequestHandler<CreateOrderCommand, bool>.Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            if(request != null && request.cartItems.Count() > 0)
            {

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
                    AddressId = request.selectedAddressDto.Id,
                    Notes = request.notes,
                    PaymentMethod = request.PaymentMethodId,
                    OrderGuid = orderGuid,
                };

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }

            return false;
        }
    }
}
