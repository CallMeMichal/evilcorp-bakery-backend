using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Models.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Features.Category.Queries.GetVisibleCategory
{
    public class GetVisibleCategoryQueryHandler : IRequestHandler<GetVisibleCategoryQuery, List<CategoryDTO>>
    {
        private readonly EvilCorpBakeryAppDbContext _context;

        public GetVisibleCategoryQueryHandler(EvilCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDTO>> Handle(GetVisibleCategoryQuery request, CancellationToken cancellationToken)
        {
            var categories = await _context.Categories
                .Where(x => x.isActive == true)
                .ToListAsync(cancellationToken);

            var categoryDTOs = categories.Select(category => new CategoryDTO
            {
                Id = category.Id,
                isActive = category.isActive,
                Name = category.Name,
            });

            return categoryDTOs.ToList();

        }
    }
}
