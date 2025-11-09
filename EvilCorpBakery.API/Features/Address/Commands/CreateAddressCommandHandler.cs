using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Data.Entities;
using EvilCorpBakery.API.Middleware.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Features.Address.Commands
{
    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand>
    {
        private readonly EvilCorpBakeryAppDbContext _context;

        public CreateAddressCommandHandler(EvilCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        public async Task Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            if (request?.address == null)
            {
                throw new BadRequestException("Address data is required");
            }

            var userExists = await _context.Users
                .AnyAsync(u => u.Id == request.address.Id, cancellationToken);

            if (!userExists)
            {
                throw new NotFoundException($"User with ID {request.address.Id} not found");
            }

            var entityAddress = new Data.Entities.Address
            {
                UserId = request.address.Id,
                Street = request.address.Street,
                City = request.address.City,
                PostalCode = request.address.PostalCode,
                Country = request.address.Country,
                IsDefault = request.address.IsDefault,
                Label = request.address.Label,
                PhoneAreaCode = request.address.PhoneAreaCode,
                PhoneNumber = request.address.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.Addresses.AddAsync(entityAddress, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}