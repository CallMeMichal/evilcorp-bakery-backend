using MediatR;

namespace EvilCorpBakery.API.Features.Products.CreateProduct
{
    public record CreateProductCommand(string Name, decimal Price, int Stock, string ImageUrl) : IRequest<int>;
}
