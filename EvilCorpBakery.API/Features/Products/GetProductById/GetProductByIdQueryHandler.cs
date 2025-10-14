using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Data.Entities;
using EvilCorpBakery.API.Extensions;
using EvilCorpBakery.API.Middleware.Exceptions;
using EvilCorpBakery.API.Models.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Features.Products.GetProductById
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
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.id);

            if(product == null)
            {
                throw new NotFoundException();
            }

            var productCategoryId = product.CategoryId;

            var productCategoryName = await _context.Categories.Where(x => x.Id == product.CategoryId).Select(x => x.Name).FirstOrDefaultAsync();

            if(productCategoryName == null)
            {
                productCategoryName = "undefined";
            }

            ProductDTO productDTO = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Category = productCategoryName,
                Base64Image = await PhotoConverter.ConvertImageToBase64Async(product.ImageUrl)
            };



            return productDTO;
        }
    }
}
