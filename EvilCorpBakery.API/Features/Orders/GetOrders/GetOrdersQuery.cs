using EvilCorpBakery.API.Data.Entities;
using EvilCorpBakery.API.Models.DTO;
using MediatR;

namespace EvilCorpBakery.API.Features.Orders.GetOrders
{
    public record GetOrdersQuery(int userId) : IRequest<List<OrderDto>>;
}
