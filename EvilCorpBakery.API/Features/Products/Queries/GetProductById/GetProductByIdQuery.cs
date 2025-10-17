using EvilCorpBakery.API.Data.Entities;
using EvilCorpBakery.API.Models.DTO;
using MediatR;

namespace EvilCorpBakery.API.Features.Products.Queries.GetProductById
{
    public record GetProductByIdQuery(int id) : IRequest<ProductDTO>;
}
