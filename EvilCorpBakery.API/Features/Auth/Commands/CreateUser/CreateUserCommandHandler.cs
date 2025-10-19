using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Data.Entities;
using EvilCorpBakery.API.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Features.Auth.Command.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
    {
        private readonly EvilCorpBakeryAppDbContext _context;

        public CreateUserCommandHandler(EvilCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if (existingUser != null)
            {
                throw new Exception("User already registered");
            }

            var user = new Data.Entities.User
            {
                Email = request.Email,
                Password = request.Password,
                Name = request.Name,
                Surname = request.Surname,
                DateOfBirth = request.DateOfBirth,
            };

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            //var token = JwtTokenGenerator.GenerateToken(user);

            return true;
        }
    }
}
