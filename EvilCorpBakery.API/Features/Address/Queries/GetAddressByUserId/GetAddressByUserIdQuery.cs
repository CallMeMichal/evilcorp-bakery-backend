using EvilCorpBakery.API.Models.DTO;
using MediatR;

namespace EvilCorpBakery.API.Features.Address.Queries.GetAddressByUserId
{
    public record GetAddressByUserIdQuery(int userId) : IRequest<List<AddressDTO>>;
}
