using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Data.Entities;
using EvilCorpBakery.API.Extensions;
using EvilCorpBakery.API.Features.Auth.LoginUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Features.Auth.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly EvliCorpBakeryAppDbContext _context;

        public CreateUserCommandHandler(EvliCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        async Task<string> IRequestHandler<CreateUserCommand, string>.Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if (existingUser != null)
            {
                throw new Exception("User already registered");
            }

            var user = new User
            {
                Email = request.Email,
                Password = request.Password,
                Name = request.Name,
                Surname = request.Surname,
                PhoneNumber = request.PhoneNumber,
                DateOfBirth = request.DateOfBirth,
            };

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var token = JwtTokenGenerator.GenerateToken(user);

            return token;
        }
    }
}
