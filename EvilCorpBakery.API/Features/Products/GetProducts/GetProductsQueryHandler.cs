using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Data.Entities;
using EvilCorpBakery.API.Extensions;
using EvilCorpBakery.API.Models.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Features.Products.GetProducts
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
                var productCategoryName = await _context.Categories.Where(x => x.Id == product.CategoryId).Select(x => x.Name).FirstOrDefaultAsync();
                if(productCategoryName == null)
                { 
                    productCategoryName = "undefined";
                }
                var productDTO = new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock,
                    Category = productCategoryName,
                    Base64Image = string.Empty
                };

                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    try
                    {
                        productDTO.Base64Image = await PhotoConverter.ConvertImageToBase64Async(product.ImageUrl);
                    }
                    catch (Exception ex)
                    {
                        productDTO.Base64Image = string.Empty;
                    }
                }

                productDTOs.Add(productDTO);
            }

            return productDTOs;
        }
    }
}
