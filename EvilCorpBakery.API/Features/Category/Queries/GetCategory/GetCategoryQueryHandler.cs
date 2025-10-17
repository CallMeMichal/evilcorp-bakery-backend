using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Models.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Features.Category.Queries.GetCategory
{
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, List<CategoryDTO?>>
    {
        private readonly EvilCorpBakeryAppDbContext _context;

        public GetCategoryQueryHandler(EvilCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        async Task<List<CategoryDTO?>> IRequestHandler<GetCategoryQuery, List<CategoryDTO?>>.Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var categories = await _context.Categories.ToListAsync();
            var categoryDTOs = categories.Select(category => new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                isActive = category.isActive
            }).ToList();


            return categoryDTOs;
        }
    }
}
