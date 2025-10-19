using MediatR;

namespace EvilCorpBakery.API.Features.Products.Command.UpdateProduct
{
    public record UpdateProductCommand(int Id, string Name, decimal Price, int Stock, string ImageUrl, string Description) : IRequest<int>;
}
