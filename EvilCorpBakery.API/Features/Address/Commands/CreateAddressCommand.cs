using EvilCorpBakery.API.Models;
using EvilCorpBakery.API.Models.DTO;
using MediatR;

namespace EvilCorpBakery.API.Features.Address.Commands
{
    public record CreateAddressCommand(AddressDTO address) : IRequest;
}
