using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Extensions;
using EvilCorpBakery.API.Models.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Features.Products.Queries.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDTO>>
    {
        private readonly EvilCorpBakeryAppDbContext _context;

        public GetProductsQueryHandler(EvilCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _context.Products.ToListAsync(cancellationToken);
            var productDTOs = new List<ProductDTO>();

            foreach (var product in products)
            {
                var productCategoryName = await _context.Categories
                    .Where(x => x.Id == product.CategoryId)
                    .Select(x => x.Name)
                    .FirstOrDefaultAsync(cancellationToken) ?? "undefined";

                var photoDtos = new List<ProductPhotosDTO>();

                foreach (var photo in product.Photos)
                {
                    try
                    {
                        var base64Image = await FileHelper.ConvertImageToBase64Async(photo.Url);
                        photoDtos.Add(new ProductPhotosDTO
                        {
                            Id = photo.Id,
                            Url = base64Image,
                            isMain = photo.IsMain
                        });
                    }
                    catch (Exception ex)
                    {
                    }
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

                productDTOs.Add(productDTO);
            }

            return productDTOs;
        }
    }
}
