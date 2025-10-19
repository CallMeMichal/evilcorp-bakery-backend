using EvilCorpBakery.API.Models.Domain;
using MediatR;

namespace EvilCorpBakery.API.Features.Orders.Command.CreateOrder
{
    public record CreateOrderCommand(string deliveryMethod, int userId, SelectedAddressDomain? selectedAddressDto, int PaymentMethodId, List<OrderItemDomain> cartItems, decimal total, string? notes) : IRequest<bool>;
}
