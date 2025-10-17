using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Data.Entities;
using EvilCorpBakery.API.Extensions;
using EvilCorpBakery.API.Middleware.Exceptions;
using EvilCorpBakery.API.Models.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDTO>
    {
        private readonly EvilCorpBakeryAppDbContext _context;

        public GetProductByIdQueryHandler(EvilCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == request.id, cancellationToken);

            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }

            var productCategoryName = await _context.Categories
                .Where(x => x.Id == product.CategoryId)
                .Select(x => x.Name)
                .FirstOrDefaultAsync(cancellationToken) ?? "undefined";

            var photoDtos = new List<ProductPhotosDTO>();

            foreach (var photo in product.Photos)
            {
                var base64Image = await FileHelper.ConvertImageToBase64Async(photo.Url);
                photoDtos.Add(new ProductPhotosDTO
                {
                    Id = photo.Id,
                    Url = base64Image,
                    isMain = photo.IsMain
                });
            }

            var productDTO = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Category = productCategoryName,
                Photos = photoDtos
            };

            return productDTO;
        }
    }
}
