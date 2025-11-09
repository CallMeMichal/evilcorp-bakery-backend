using EvilCorpBakery.API.Middleware.Exceptions;
using EvilCorpBakery.API.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace EvilCorpBakery.API.Middleware
{
    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiResponseMiddleware> _logger;

        public ApiResponseMiddleware(RequestDelegate next, ILogger<ApiResponseMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (ShouldSkipMiddleware(context.Request.Path))
            {
                await _next(context);
                return;
            }

            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);

                if (context.Response.StatusCode < 400 && IsApiEndpoint(context))
                {
                    await HandleSuccessResponse(context, originalBodyStream);
                }
                else
                {
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionResponse(context, ex, originalBodyStream);
            }
        }

        private static bool IsApiEndpoint(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase);
        }

        private static bool ShouldSkipMiddleware(PathString path)
        {
            var pathsToSkip = new[]
            {
                "/openapi",
                "/scalar",
                "/_framework",
                "/_vs",
                "/swagger",
                "/favicon.ico",
                "/health",
                "/metrics"
            };

            return pathsToSkip.Any(skipPath => path.StartsWithSegments(skipPath, StringComparison.OrdinalIgnoreCase));
        }

        private async Task HandleSuccessResponse(HttpContext context, Stream originalBodyStream)
        {
            try
            {
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseBodyText = await new StreamReader(context.Response.Body, Encoding.UTF8).ReadToEndAsync();

                object wrappedResponse;

                if (string.IsNullOrWhiteSpace(responseBodyText) || context.Response.StatusCode == 204)
                {
                    wrappedResponse = new ApiResponse
                    {
                        Success = true,
                        Status = context.Response.StatusCode,
                        Title = "Success",
                        Detail = "Operation completed successfully",
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.3.1", // Lub odpowiedni link
                        Instance = context.Request.Path
                    };
                }
                else
                {
                    try
                    {
                        var data = JsonSerializer.Deserialize<JsonElement>(responseBodyText);
                        wrappedResponse = new ApiResponse<JsonElement>
                        {
                            Success = true,
                            Status = context.Response.StatusCode,
                            Title = "Success",
                            Detail = "Operation completed successfully",
                            Data = data,
                            Type = "https://tools.ietf.org/html/rfc7231#section-6.3.1",
                            Instance = context.Request.Path
                        };
                    }
                    catch (JsonException)
                    {
                        wrappedResponse = new ApiResponse<string>
                        {
                            Success = true,
                            Status = context.Response.StatusCode,
                            Title = "Success",
                            Detail = "Operation completed successfully",
                            Data = responseBodyText,
                            Type = "https://tools.ietf.org/html/rfc7231#section-6.3.1",
                            Instance = context.Request.Path
                        };
                    }
                }

                await WriteJsonResponse(context, wrappedResponse, originalBodyStream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while handling success response");
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await context.Response.Body.CopyToAsync(originalBodyStream);
            }
        }

        private async Task HandleExceptionResponse(HttpContext context, Exception ex, Stream originalBodyStream)
        {
            try
            {
                context.Response.Clear();

                var statusCode = GetStatusCodeFromException(ex);
                context.Response.StatusCode = statusCode;

                var response = new ApiResponse
                {
                    Success = false,
                    Status = statusCode,
                    Title = GetErrorTitle(statusCode),
                    Detail = GetErrorDetail(ex),
                    Type = GetErrorType(statusCode),
                    Instance = context.Request.Path
                };

                await WriteJsonResponse(context, response, originalBodyStream);
            }
            catch (Exception writeEx)
            {
                _logger.LogError(writeEx, "Error occurred while writing error response");

                try
                {
                    context.Response.StatusCode = 500;
                    var fallbackResponse = "Internal Server Error";
                    await originalBodyStream.WriteAsync(Encoding.UTF8.GetBytes(fallbackResponse));
                }
                catch
                {
                    //todo
                }
            }
        }

        private async Task WriteJsonResponse(HttpContext context, object response, Stream originalBodyStream)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            var jsonResponse = JsonSerializer.Serialize(response, jsonOptions);
            var responseBytes = Encoding.UTF8.GetBytes(jsonResponse);

            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.ContentLength = responseBytes.Length;

            await originalBodyStream.WriteAsync(responseBytes);
        }

        private static int GetStatusCodeFromException(Exception ex) => ex switch
        {
            TokenExpiredException => 401,
            NotFoundException => 404,
            Exceptions.ValidationException => 400,
            BadRequestException => 400,
            ConflictException => 409,
            UnauthorizedAccessException => 401,
            ArgumentException => 400,
            ForbiddenException => 403,
            InvalidOperationException => 400,
            NotSupportedException => 501,
            TimeoutException => 408,
            _ => 500
        };

        private static string GetErrorTitle(int statusCode) => statusCode switch
        {
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            405 => "Method Not Allowed",
            408 => "Request Timeout",
            409 => "Conflict",
            422 => "Unprocessable Entity",
            500 => "Internal Server Error",
            501 => "Not Implemented",
            502 => "Bad Gateway",
            503 => "Service Unavailable",
            _ => "Error"
        };

        private static string GetErrorType(int statusCode) => statusCode switch
        {
            400 => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            401 => "https://tools.ietf.org/html/rfc7235#section-3.1",
            403 => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            404 => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            405 => "https://tools.ietf.org/html/rfc7231#section-6.5.5",
            408 => "https://tools.ietf.org/html/rfc7231#section-6.5.7",
            409 => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            422 => "https://tools.ietf.org/html/rfc4918#section-11.2",
            500 => "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            501 => "https://tools.ietf.org/html/rfc7231#section-6.6.2",
            502 => "https://tools.ietf.org/html/rfc7231#section-6.6.3",
            503 => "https://tools.ietf.org/html/rfc7231#section-6.6.4",
            _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

        private static string GetErrorDetail(Exception ex)
        {
            // W środowisku produkcyjnym możesz chcieć ukryć szczegóły błędów
            return ex switch
            {
                TokenExpiredException tokenEx => tokenEx.Message,
                Exceptions.ValidationException validationEx => validationEx.Message,
                NotFoundException notFoundEx => notFoundEx.Message,
                BadRequestException badRequestEx => badRequestEx.Message,
                ArgumentException argumentEx => argumentEx.Message,
                _ => "An error occurred while processing your request."
            };
        }
    }
}