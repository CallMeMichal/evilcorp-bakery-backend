using MediatR;

namespace EvilCorpBakery.API.Features.Auth.Command.LoginUser
{
    public record LoginUserCommand(string Email, string Password) : IRequest<List<string>>;
}
