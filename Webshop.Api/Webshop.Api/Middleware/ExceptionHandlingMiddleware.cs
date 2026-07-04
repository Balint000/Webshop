using System.Net;
using System.Text.Json;
using Webshop.Api.Responses;

namespace Webshop.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _enviroment;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IWebHostEnvironment enviroment)
        {
            _next = next;
            _logger = logger;
            _enviroment = enviroment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

            }catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
                
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                KeyNotFoundException => HttpStatusCode.NotFound,
                ArgumentException => HttpStatusCode.BadRequest,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;

            var response = new ApiErrorResponse
            {
                StatusCode = context.Response.StatusCode,
                Message = statusCode switch
                {
                    HttpStatusCode.NotFound => "A kért elérhetőség nem található.",
                    HttpStatusCode.BadRequest => "Hibás kérés.",
                    HttpStatusCode.Unauthorized => "Nem rendelkezik jogosultsággal a művelet végrehajtásához.",
                    _ => "Váratlan hiba történt."
                },

                Details = _enviroment.IsDevelopment() ? exception.Message : null
            };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}
