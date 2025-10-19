using EvilCorpBakery.API.Features.Auth.Command.CreateUser;
using Shouldly;
namespace EvilCorpBakery.UnitTests.Auth.Commands
{
    public class CreateUserCommandHandlerTest : CommandTestBase
    {
        private readonly CreateUserCommandHandler _handler;

        public CreateUserCommandHandlerTest()
            : base()
        {
            _handler = new CreateUserCommandHandler(_context);
        }


        [Fact]
        public async Task Handle_GivenValidRequest_ShouldCreateUser()
        {

            var command = new CreateUserCommand(
                Name: "John",
                Surname: "Doe",
                Email: "johndoe12@example.com",
                Password: "Password123!",
                DateOfBirth: new DateTime(1990, 1, 1),
                PhoneNumber: "1234567890"
                );

            var result = await _handler.Handle(command, CancellationToken.None);


            result.ShouldBeTrue();
        }
    }

}
