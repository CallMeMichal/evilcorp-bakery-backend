using EvilCorpBakery.API.Models.DTO;
using MediatR;

namespace EvilCorpBakery.API.Features.User.Command.UpdateUser
{
    public record UpdateUserCommand(int id, UserDTO userDto) : IRequest<bool>;
}
