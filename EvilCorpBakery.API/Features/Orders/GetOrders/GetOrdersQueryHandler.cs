using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Data.Entities;
using EvilCorpBakery.API.Extensions;
using EvilCorpBakery.API.Middleware.Exceptions;
using EvilCorpBakery.API.Models.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Features.Orders.GetOrders
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<OrderDto>>
    {
        private readonly EvilCorpBakeryAppDbContext _context;

        public GetOrdersQueryHandler(EvilCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Where(u => u.Id == request.userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            var orders = await _context.Orders
                .Where(o => o.UserId == request.userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync(cancellationToken);

            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                var itemDtos = new List<OrderItemDto>();

                foreach (var item in order.OrderItems)
                {
                    var base64Image = await PhotoConverter.ConvertImageToBase64Async(item.Product.ImageUrl);

                    itemDtos.Add(new OrderItemDto
                    {
                        Id = item.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        ProductDTO = new ProductDTO
                        {
                            Id = item.Product.Id,
                            Name = item.Product.Name,
                            Description = item.Product.Description,
                            Price = item.Product.Price,
                            Stock = item.Product.Stock,
                            Base64Image = base64Image
                        }
                    });
                }

                var orderStatus = await _context.OrderStatuses.Where(x=>x.Id == order.StatusId).FirstOrDefaultAsync(cancellationToken);

                orderDtos.Add(new OrderDto
                {
                    Id = order.Id,
                    TotalAmount = order.TotalAmount,
                    OrderGuid = order.OrderGuid,
                    Status = orderStatus != null ? orderStatus.Name : "Unknown",
                    Notes = order.Notes ?? string.Empty,
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt,
                    Items = itemDtos
                });
            }

            return orderDtos;
        }
    }
}