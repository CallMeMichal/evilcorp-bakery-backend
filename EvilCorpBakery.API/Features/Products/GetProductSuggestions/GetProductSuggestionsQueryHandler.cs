using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Data.Entities;
using EvilCorpBakery.API.Extensions;
using EvilCorpBakery.API.Models.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Features.Products.GetProductSuggestions
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

            var products = await _context.Products
                .Where(p => p.Name.ToLower().Contains(searchTerm))
                .OrderBy(p => p.Name.ToLower().IndexOf(searchTerm))
                .ThenBy(p => p.Name)
                .Take(10)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.ImageUrl,
                    p.Stock
                })
                .ToListAsync(cancellationToken);

            var suggestions = new List<ProductDTO>();

            foreach (var product in products)
            {
                var base64Image = await PhotoConverter.ConvertImageToBase64Async(product.ImageUrl);

                suggestions.Add(new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Base64Image = base64Image,
                });
            }

            return suggestions;

        }
    }
}
