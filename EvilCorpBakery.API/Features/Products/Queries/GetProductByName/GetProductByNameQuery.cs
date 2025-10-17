using EvilCorpBakery.API.Data.Entities;
using MediatR;

namespace EvilCorpBakery.API.Features.Products.Queries.GetProductByName
{
    public record GetProductByNameQuery(string Name) : IRequest<Product>;
}
