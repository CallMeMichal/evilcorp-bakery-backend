using EvilCorpBakery.API.Data.Entities;
using EvilCorpBakery.API.Models.DTO;
using MediatR;

namespace EvilCorpBakery.API.Features.Products.GetProducts
{
    public record GetProductsQuery() : IRequest<List<ProductDTO>>;
}
