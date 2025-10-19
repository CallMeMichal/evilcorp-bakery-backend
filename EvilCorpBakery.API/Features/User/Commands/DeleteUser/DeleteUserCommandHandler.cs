using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Middleware.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Features.User.Command.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly EvilCorpBakeryAppDbContext _context;

        public DeleteUserCommandHandler(EvilCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.id);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            if (user.Role == "Admin" && user.isActive == true)
            {
                var activeAdminCount = await _context.Users
                    .CountAsync(x => x.Role == "Admin" && x.isActive == true);

                if (activeAdminCount <= 1)
                {
                    throw new InvalidOperationException("Cannot delete the last active administrator. At least one administrator must remain in the system.");
                }
            }

            // Dezaktywuj użytkownika (poprawka: = zamiast .Equals())
            user.isActive = false;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
