using EvilCorpBakery.API.Models.DTO;
using MediatR;

namespace EvilCorpBakery.API.Features.Products.Queries.GetProducts
{
    public record GetProductsQuery : IRequest<List<ProductDTO>>;
}
