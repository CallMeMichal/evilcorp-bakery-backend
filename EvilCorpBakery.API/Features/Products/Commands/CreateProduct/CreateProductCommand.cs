using EvilCorpBakery.API.Models.DTO;
using MediatR;

namespace EvilCorpBakery.API.Features.Products.Command.CreateProduct
{
    public record CreateProductCommand(string Name, string Description, decimal Price, int Stock, string CategoryName, List<ProductPhotosDTO> Photos) : IRequest<bool>;
}
