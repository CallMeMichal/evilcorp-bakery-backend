using EvilCorpBakery.API.Features.Auth.Command.CreateUser;
using EvilCorpBakery.API.Features.Auth.Command.LoginUser;
using EvilCorpBakery.API.Data.Entities;
using Xunit;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace EvilCorpBakery.UnitTests.Auth.Commands
{
    public class LoginUserCommandHandlerTest : CommandTestBase
    {
        private readonly LoginUserCommandHandler _handler;
        private readonly CreateUserCommandHandler _createUserHandler;
        public LoginUserCommandHandlerTest() 
            : base()
        {
            _handler = new LoginUserCommandHandler(_context);
            _createUserHandler = new CreateUserCommandHandler(_context);
        }

        [Fact]
        public async Task Handle_GivenValidRequest_ShouldReturnToken()
        {
            var command = new CreateUserCommand(
                Name: "John",
                Surname: "Doe",
                Email: "johndoe@example.com",
                Password: "Password123!",
                DateOfBirth: new DateTime(1990, 1, 1),
                PhoneNumber: "1234567890"
                );

            var createdUserResult = await _createUserHandler.Handle(command, CancellationToken.None);

            var loginCommand = new LoginUserCommand(
                Email: "johndoe@example.com",
                Password: "Password123!"
            );

            var result = await _handler.Handle(loginCommand, CancellationToken.None);

            Assert.NotNull(result);

        }
    }
}