using EvilCorpBakery.API.Models;
using EvilCorpBakery.API.Models.Domain;
using EvilCorpBakery.API.Models.DTO;
using MediatR;

namespace EvilCorpBakery.API.Features.Orders.Command.CreateOrder
{
    public record CreateOrderCommand(string deliveryMethod, int userId, SelectedAddressDomain? selectedAddressDto, int PaymentMethodId, List<OrderItemDomain>? cartItems, decimal total, string? notes) : IRequest<OrderCreatedDto>;
}
