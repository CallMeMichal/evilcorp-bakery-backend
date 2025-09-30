namespace EvilCorpBakery.API.Middleware.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message = "Access denied")
            : base(message)
        {
        }
    }
}