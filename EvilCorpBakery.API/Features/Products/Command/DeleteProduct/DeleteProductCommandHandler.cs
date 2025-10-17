using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Features.Products.CreateProduct;
using EvilCorpBakery.API.Middleware.Exceptions;
using MediatR;

namespace EvilCorpBakery.API.Features.Products.Command.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, int>
    {
        private readonly EvilCorpBakeryAppDbContext _context;

        public DeleteProductCommandHandler(EvilCorpBakeryAppDbContext context)
        {
            _context = context;
        }


        public async Task<int> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);

            if (product == null)
            {
                throw new NotFoundException("Product", request.Id);
            }

            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync(cancellationToken);
            }


            return request.Id;
        }
    }
}
