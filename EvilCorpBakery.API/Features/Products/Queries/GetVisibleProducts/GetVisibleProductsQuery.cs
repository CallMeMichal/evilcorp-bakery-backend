using EvilCorpBakery.API.Data.Entities;
using EvilCorpBakery.API.Models.DTO;
using MediatR;

namespace EvilCorpBakery.API.Features.Products.Queries.GetVisibleProducts
{
    public record GetVisibleProductsQuery() : IRequest<List<ProductDTO>>;
}
