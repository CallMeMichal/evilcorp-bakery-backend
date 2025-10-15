using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Models.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Features.User.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDTO>>
    {
        private readonly EvilCorpBakeryAppDbContext _context;

        public GetUsersQueryHandler(EvilCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserDTO>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _context.Users.ToListAsync(cancellationToken);
            
            var userDTOs = users.Select(user => new UserDTO
            {
                Id = user.Id,
                Surname = user.Surname,
                CreatedAt = user.CreatedAt,
                DateOfBirth = user.DateOfBirth,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            }).ToList();

            return userDTOs;
        }
    }
}
