using EvilCorpBakery.API.Data.Entities;
using MediatR;

namespace EvilCorpBakery.API.Features.Products.GetProductById
{
    public record GetProductByIdQuery(int Id) : IRequest<Product>;
}
