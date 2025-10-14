using MediatR;

namespace EvilCorpBakery.API.Features.Auth.GetUserJoinDate
{
    public record GetUserJoinDateQuery(int userId) : IRequest<DateTime>;
}
