using MediatR;

namespace EvilCorpBakery.API.Features.Auth.Queries.GetUserJoinDate
{
    public record GetUserJoinDateQuery(int userId) : IRequest<DateTime>;
}
