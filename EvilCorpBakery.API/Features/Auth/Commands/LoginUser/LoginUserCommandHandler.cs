using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Extensions;
using MediatR;

namespace EvilCorpBakery.API.Features.Auth.Command.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, List<string>>
    {
        private readonly EvilCorpBakeryAppDbContext _context;

        public LoginUserCommandHandler(EvilCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        public Task<List<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = _context.Users.Where(x=>x.isActive == true).FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);

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
