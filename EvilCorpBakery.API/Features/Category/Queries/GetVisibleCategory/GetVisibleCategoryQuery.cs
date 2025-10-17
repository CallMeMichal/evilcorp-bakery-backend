using EvilCorpBakery.API.Models.DTO;
using MediatR;

namespace EvilCorpBakery.API.Features.Category.Queries.GetVisibleCategory
{
    public class GetVisibleCategoryQuery : IRequest<List<CategoryDTO>>;
}
