using MediatR;

namespace EvilCorpBakery.API.Features.User.Command.DeleteUser
{
    public record DeleteUserCommand(int id) : IRequest<bool>;
}
