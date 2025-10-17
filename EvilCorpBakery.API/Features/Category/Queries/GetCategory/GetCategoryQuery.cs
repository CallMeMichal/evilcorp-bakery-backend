using EvilCorpBakery.API.Models.DTO;
using MediatR;

namespace EvilCorpBakery.API.Features.Category.Queries.GetCategory
{
    public record GetCategoryQuery : IRequest<List<CategoryDTO?>>;

}
