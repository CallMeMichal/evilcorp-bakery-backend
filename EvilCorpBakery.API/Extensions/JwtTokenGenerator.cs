using EvilCorpBakery.API.Data.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EvilCorpBakery.API.Extensions
{
    public static class JwtTokenGenerator
    {

        private const string JWT_SECRET = "super-tajny-klucz-evilcorp-bakery-2024-minimum-32-znaki";
        private const string JWT_ISSUER = "EvilCorpBakery";
        private const string JWT_AUDIENCE = "https://evilcorpbakery.com";

        public static string GenerateToken(User user)
        {
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWT_SECRET)),
                SecurityAlgorithms.HmacSha256);

            if (user == null ||
                user.Id == 0 ||
                string.IsNullOrWhiteSpace(user.Name) ||
                string.IsNullOrWhiteSpace(user.Surname) ||
                string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.Role))
            {
                throw new InvalidOperationException("User data is incomplete or null.");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Name),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.Surname),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("role", user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var secToken = new JwtSecurityToken(
                issuer: JWT_ISSUER,
                audience: JWT_AUDIENCE,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials,
                claims: claims
            );

            return new JwtSecurityTokenHandler().WriteToken(secToken);
        }
    }
}
