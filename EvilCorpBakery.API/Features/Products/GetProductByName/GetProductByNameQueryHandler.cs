using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Data.Entities;
using EvilCorpBakery.API.Middleware.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Features.Products.GetProductByName
{
    public class GetProductByNameQueryHandler : IRequestHandler<GetProductByNameQuery, Product>
    {
        private readonly EvilCorpBakeryAppDbContext _context;

        public GetProductByNameQueryHandler(EvilCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Name == request.Name);

            if(product == null)
            {
                throw new NotFoundException();
            }

            return product;
        }
    }
}
