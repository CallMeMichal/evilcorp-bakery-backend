using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Models.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Features.Address.GetAddressByUserId
{
    public class GetAddressByUserIdQueryHandler : IRequestHandler<GetAddressByUserIdQuery, List<AddressDTO>>
    {
        private readonly EvilCorpBakeryAppDbContext _context;

        public GetAddressByUserIdQueryHandler(EvilCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AddressDTO>> Handle(GetAddressByUserIdQuery request, CancellationToken cancellationToken)
        {
            var addresses = await _context.Addresses
                .Where(a => a.UserId == request.userId)
                .Select(a => new AddressDTO
                {
                    Id = a.Id,
                    Street = a.Street,
                    City = a.City,
                    PostalCode = a.PostalCode,
                    Country = a.Country,
                    IsDefault = a.IsDefault,
                    Label = a.Label,
                    PhoneNumber = a.PhoneNumber,
                    PhoneAreaCode = a.PhoneAreaCode
                })
                .ToListAsync(cancellationToken);

            return addresses;
        }
    }
}
