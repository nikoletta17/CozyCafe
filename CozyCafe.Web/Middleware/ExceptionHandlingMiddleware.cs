using CozyCafe.Application.Exceptions;
using System.Net;
using System.Text.Json;


namespace CozyCafe.Web.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        /// <summary>
        /// (UA) Middleware для централізованої обробки винятків у додатку. 
        /// Перехоплює всі необроблені помилки, логгує їх та повертає стандартизовану 
        /// JSON-відповідь з відповідним HTTP-статусом.
        /// 
        /// (EN) Middleware for centralized exception handling in the application. 
        /// Catches all unhandled errors, logs them, and returns a standardized 
        /// JSON response with the appropriate HTTP status code.
        /// </summary>

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = context.Response;
            var statusCode = exception switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,           // 404
                ValidationException => (int)HttpStatusCode.BadRequest,       // 400
                UnauthorizedException => (int)HttpStatusCode.Unauthorized,   // 401
                ConflictException => (int)HttpStatusCode.Conflict,           // 409
                _ => (int)HttpStatusCode.InternalServerError                 // 500
            };

            var errorResponse = new
            {
                code = exception.GetType().Name, // "NotFoundException", "CartEmptyException", etc
                message = exception.Message,
                details = (exception as AppException)?.AdditionalData
            };

            response.StatusCode = statusCode;

            _logger.LogError(exception, "Unhandled exception occurred");

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
