using MediatR;

namespace EvilCorpBakery.API.Features.Auth.Command.CreateUser
{
    public record CreateUserCommand(string Name, string Surname, string Email, string Password, DateTime DateOfBirth, string PhoneNumber) : IRequest<bool>;
}
