using MediatR;

namespace EvilCorpBakery.API.Features.Category.Command.SetVisibleCategory
{
    public record SetVisibleCategoryCommand(int id) : IRequest<bool>;

}
