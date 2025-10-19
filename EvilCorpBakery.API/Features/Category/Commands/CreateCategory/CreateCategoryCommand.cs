using MediatR;

namespace EvilCorpBakery.API.Features.Category.Command.CreateCategory
{
    public record CreateCategoryCommand(string name) : IRequest<bool>;
}
