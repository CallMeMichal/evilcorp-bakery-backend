using EvilCorpBakery.API.Data;
using MediatR;

namespace EvilCorpBakery.API.Features.Category.Command.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, bool>
    {
        private readonly EvilCorpBakeryAppDbContext _context;

        public CreateCategoryCommandHandler(EvilCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Data.Entities.Category
            {
                Name = request.name,
                CreatedAt = DateTime.UtcNow,
            };

            await _context.Categories.AddAsync(category);
            _context.SaveChanges();
            return true;
        }
    }
}
