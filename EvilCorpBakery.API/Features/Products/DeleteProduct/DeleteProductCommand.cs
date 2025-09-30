using MediatR;

namespace EvilCorpBakery.API.Features.Products.DeleteProduct
{
    public record DeleteProductCommand(int Id) : IRequest<int>;

}
