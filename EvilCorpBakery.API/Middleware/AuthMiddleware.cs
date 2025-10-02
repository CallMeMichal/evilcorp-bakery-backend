// Middleware/AuthMiddleware.cs
using EvilCorpBakery.API.Middleware.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace EvilCorpBakery.API.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var authorizeAttribute = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>();

            if (authorizeAttribute != null)
            {
                if (context.User.Identity?.IsAuthenticated != true)
                {
                    throw new TokenExpiredException(); // 401
                }

                if (!string.IsNullOrEmpty(authorizeAttribute.Roles))
                {
                    var requiredRoles = authorizeAttribute.Roles.Split(',')
                        .Select(r => r.Trim());

                    var hasRole = requiredRoles.Any(role =>
                        context.User.IsInRole(role));

                    if (!hasRole)
                    {
                        throw new ForbiddenException(
                            $"User does not have required role(s): {authorizeAttribute.Roles}"); // 403
                    }
                }
            }

            await _next(context);
        }
    }
}