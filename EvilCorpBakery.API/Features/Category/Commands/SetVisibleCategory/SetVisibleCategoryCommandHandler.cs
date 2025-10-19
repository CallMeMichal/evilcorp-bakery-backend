using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Middleware.Exceptions;
using MediatR;

namespace EvilCorpBakery.API.Features.Category.Command.SetVisibleCategory
{
    public class SetVisibleCategoryCommandHandler : IRequestHandler<SetVisibleCategoryCommand, bool>
    {
        private readonly EvilCorpBakeryAppDbContext _context;

        public SetVisibleCategoryCommandHandler(EvilCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(SetVisibleCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FindAsync(request.id);

            if (category == null)
            {
                throw new NotFoundException("Category not found");
            }

            category.isActive = true;
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
