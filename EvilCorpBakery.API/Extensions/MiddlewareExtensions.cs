using EvilCorpBakery.API.Middleware;

namespace EvilCorpBakery.API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseApiResponseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiResponseMiddleware>();
        }

        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}
