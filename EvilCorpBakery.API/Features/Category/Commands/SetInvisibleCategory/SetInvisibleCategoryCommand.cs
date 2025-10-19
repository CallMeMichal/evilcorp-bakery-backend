using MediatR;

namespace EvilCorpBakery.API.Features.Category.Command.SetInvisibleCategory
{
    public record SetInvisibleCategoryCommand(int id) : IRequest<bool>;
}
