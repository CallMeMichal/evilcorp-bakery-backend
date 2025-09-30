namespace EvilCorpBakery.API.Middleware.Exceptions
{
    public class TokenExpiredException : Exception
    {
        public TokenExpiredException()
            : base("Token is invalid or has expired")
        {
        }
    }
}
