using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Middleware.Exceptions;
using MediatR;

namespace EvilCorpBakery.API.Features.Category.Command.SetInvisibleCategory
{
    public class SetInvisibleCategoryCommandHandler : IRequestHandler<SetInvisibleCategoryCommand, bool>
    {
        private readonly EvilCorpBakeryAppDbContext _context;

        public SetInvisibleCategoryCommandHandler(EvilCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(SetInvisibleCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FindAsync(request.id);

            if (category == null)
            {
                throw new NotFoundException("Category not found");
            }

            category.isActive = false;
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
