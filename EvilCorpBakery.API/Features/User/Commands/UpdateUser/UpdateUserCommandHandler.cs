using EvilCorpBakery.API.Data;
using EvilCorpBakery.API.Data.Entities;
using EvilCorpBakery.API.Middleware.Exceptions;
using EvilCorpBakery.API.Models.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EvilCorpBakery.API.Features.User.Command.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly EvilCorpBakeryAppDbContext _context;

        public UpdateUserCommandHandler(EvilCorpBakeryAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var userDto = request.userDto;

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.id, cancellationToken);

            if (user == null)
                throw new NotFoundException("User not found");

            user.Name = userDto.Name;
            user.Surname = userDto.Surname;
            user.Email = userDto.Email;
            user.Role = userDto.Role;
            user.DateOfBirth = userDto.DateOfBirth;
            user.isActive = userDto.isActive;
            user.UpdatedAt = DateTime.UtcNow;

            if (userDto.Addresses != null)
            {
                await UpdateUserAddresses(user, userDto.Addresses, cancellationToken);
            }


            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }


        private async Task UpdateUserAddresses(Data.Entities.User user, List<AddressDTO> addressDtos, CancellationToken cancellationToken)
        {
            await _context.Entry(user)
                .Collection(u => u.Addresses)
                .LoadAsync(cancellationToken);

            var existingAddresses = user.Addresses.ToList();
            var incomingAddressIds = addressDtos.Where(a => a.Id > 0).Select(a => a.Id).ToList();

            foreach (var addressDto in addressDtos.Where(a => a.Id > 0))
            {
                var existingAddress = existingAddresses.FirstOrDefault(a => a.Id == addressDto.Id);
                if (existingAddress != null)
                {
                    existingAddress.Street = addressDto.Street ?? string.Empty;
                    existingAddress.City = addressDto.City ?? string.Empty;
                    existingAddress.PostalCode = addressDto.PostalCode ?? string.Empty;
                    existingAddress.Country = addressDto.Country ?? string.Empty;
                    existingAddress.IsDefault = addressDto.IsDefault;
                    existingAddress.Label = addressDto.Label;
                    existingAddress.PhoneAreaCode = addressDto.PhoneAreaCode ?? string.Empty;
                    existingAddress.PhoneNumber = addressDto.PhoneNumber ?? string.Empty;
                    existingAddress.UpdatedAt = DateTime.UtcNow;
                }
            }

            foreach (var addressDto in addressDtos.Where(a => a.Id == 0))
            {
                var newAddress = new Data.Entities.Address
                {
                    UserId = user.Id,
                    Street = addressDto.Street ?? string.Empty,
                    City = addressDto.City ?? string.Empty,
                    PostalCode = addressDto.PostalCode ?? string.Empty,
                    Country = addressDto.Country ?? string.Empty,
                    IsDefault = addressDto.IsDefault,
                    Label = addressDto.Label,
                    PhoneAreaCode = addressDto.PhoneAreaCode ?? string.Empty,
                    PhoneNumber = addressDto.PhoneNumber ?? string.Empty,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                user.Addresses.Add(newAddress);
            }

            var addressesToRemove = existingAddresses
                .Where(existing => !incomingAddressIds.Contains(existing.Id))
                .ToList();

            foreach (var addressToRemove in addressesToRemove)
            {
                _context.Addresses.Remove(addressToRemove);
            }

            EnsureSingleDefaultAddress(user);
        }

        private void EnsureSingleDefaultAddress(Data.Entities.User user)
        {
            var defaultAddresses = user.Addresses.Where(a => a.IsDefault).ToList();

            if (defaultAddresses.Count > 1)
            {
                // Zostaw pierwszy jako domyślny, resztę oznacz jako nie-domyślne
                foreach (var addr in defaultAddresses.Skip(1))
                {
                    addr.IsDefault = false;
                }
            }
            else if (defaultAddresses.Count == 0 && user.Addresses.Any())
            {
                // Jeśli żaden adres nie jest domyślny, ustaw pierwszy jako domyślny
                var firstAddress = user.Addresses.First();
                firstAddress.IsDefault = true;
            }
        }
    }
}
