using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Middleware.Exceptions;
using MediatR;

namespace EvilCorpBakery.API.Features.Products.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
    {
        private readonly EvliCorpBakeryAppDbContext _context;

        public UpdateProductCommandHandler(EvliCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);

            if (product == null)
            {
                throw new NotFoundException($"Product with ID {request.Id} not found");
            }

            product.Name = request.Name;
            product.Price = request.Price;
            product.Stock = request.Stock;
            product.Description = request.Description;
            product.ImageUrl = request.ImageUrl;

            await _context.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
