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
        private readonly EvliCorpBakeryAppDbContext _context;

        public GetProductsQueryHandler(EvliCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _context.Products.ToListAsync(cancellationToken);
            var productDTOs = new List<ProductDTO>();

            foreach (var product in products)
            {
                var productDTO = new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock,
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
