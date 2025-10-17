using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Data.Entities;
using EvilCorpBakery.API.Extensions;
using EvilCorpBakery.API.Models.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Features.Products.Queries.GetProductSuggestions
{
    public class GetProductSuggestionsQueryHandler : IRequestHandler<GetProductSuggestionsQuery, List<ProductDTO>>
    {
        private readonly EvilCorpBakeryAppDbContext _context;

        public GetProductSuggestionsQueryHandler(EvilCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDTO>> Handle(GetProductSuggestionsQuery request, CancellationToken cancellationToken)
        {
            var searchTerm = request.SearchTerm.ToLower().Trim();

            var productsWithMainPhotos = await _context.Products
                .Where(p => p.Name.ToLower().Contains(searchTerm))
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.Stock,
                    MainPhoto = p.Photos.FirstOrDefault(ph => ph.IsMain)
                })
                .OrderBy(p => p.Name.ToLower().IndexOf(searchTerm))
                .ThenBy(p => p.Name)
                .Take(10)
                .ToListAsync(cancellationToken);

            var suggestions = new List<ProductDTO>();

            foreach (var item in productsWithMainPhotos)
            {
                var photoDtos = new List<ProductPhotosDTO>();

                if (item.MainPhoto != null)
                {
                    var base64Image = await FileHelper.ConvertImageToBase64Async(item.MainPhoto.Url);
                    photoDtos.Add(new ProductPhotosDTO
                    {
                        Id = item.MainPhoto.Id,
                        Url = base64Image,
                        isMain = true
                    });
                }

                suggestions.Add(new ProductDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    Stock = item.Stock,
                    Photos = photoDtos
                });
            }

            return suggestions;
        }
    }
}
