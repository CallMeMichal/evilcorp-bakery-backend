using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Middleware.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Features.Auth.Queries.GetUserJoinDate
{
    public class GetUserJoinDateQueryHandler : IRequestHandler<GetUserJoinDateQuery, DateTime>
    {
        private readonly EvilCorpBakeryAppDbContext _context;

        public GetUserJoinDateQueryHandler(EvilCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        public async Task<DateTime> Handle(GetUserJoinDateQuery request, CancellationToken cancellationToken)
        {

            return await _context.Users
            .Where(u => u.Id == request.userId)
            .Select(u => u.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
