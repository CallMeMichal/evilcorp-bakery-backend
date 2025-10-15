using MediatR;

namespace EvilCorpBakery.API.Features.User.DeleteUser
{
    public record DeleteUserCommand(int id) : IRequest<bool>;
}
