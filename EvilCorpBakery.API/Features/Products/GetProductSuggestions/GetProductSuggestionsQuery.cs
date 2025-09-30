using EvilCorpBakery.API.Models.DTO;
using MediatR;

namespace EvilCorpBakery.API.Features.Products.GetProductSuggestions
{
    public record GetProductSuggestionsQuery(string SearchTerm) : IRequest<List<ProductDTO>>;
}
