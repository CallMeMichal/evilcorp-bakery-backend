using EvilCorpBakery.API.Data.Entities;
using EvilCorpBakery.API.Models.DTO;
using MediatR;

namespace EvilCorpBakery.API.Features.User.GetUsers
{
    public record GetUsersQuery : IRequest<List<UserDTO>>;
}
