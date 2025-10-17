using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Data.Entities;
using EvilCorpBakery.API.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Features.Products.Command.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, bool>
    {
        private readonly EvilCorpBakeryAppDbContext _context;

        public CreateProductCommandHandler(EvilCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var photos = new List<ProductPhoto>();

            if (request.Photos != null && request.Photos.Any())
            {
                foreach (var photoDto in request.Photos)
                {
                    string savedFilePath = null;

                    if (FileHelper.IsValidImageBase64(photoDto.Url))
                    {
                        savedFilePath = await FileHelper.SaveBase64FileAsync(
                            photoDto.Url,
                            "Photos/Products"
                        );
                    }
                    else
                    {
                        throw new Exception("Invalid image data provided.");
                    }

                    photos.Add(new ProductPhoto
                    {
                        Url = savedFilePath,
                        IsMain = photoDto.isMain
                    });
                }
            }

            var categoryId = await _context.Categories
                .Where(c => c.Name == request.CategoryName)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock,
                CategoryId = categoryId,
                Photos = photos,
                isActive = false
            };

            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
