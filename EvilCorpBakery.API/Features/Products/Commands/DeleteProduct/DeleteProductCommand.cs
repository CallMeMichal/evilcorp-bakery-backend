using MediatR;

namespace EvilCorpBakery.API.Features.Products.Command.DeleteProduct
{
    public record DeleteProductCommand(int Id) : IRequest<int>;

}
