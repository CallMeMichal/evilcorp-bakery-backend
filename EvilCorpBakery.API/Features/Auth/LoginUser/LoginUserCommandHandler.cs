using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Extensions;
using MediatR;

namespace EvilCorpBakery.API.Features.Auth.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, List<string>>
    {
        private readonly EvliCorpBakeryAppDbContext _context;

        public LoginUserCommandHandler(EvliCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        public Task<List<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var token = JwtTokenGenerator.GenerateToken(user);

            List<string> tokenList = new List<string>();
            tokenList.Add(token);

            return Task.FromResult(tokenList);

        }
    }
}
