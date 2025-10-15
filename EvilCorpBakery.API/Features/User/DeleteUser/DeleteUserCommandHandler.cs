using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Middleware.Exceptions;
using MediatR;

namespace EvilCorpBakery.API.Features.User.DeleteUser
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

            _context.Users.Remove(user);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
