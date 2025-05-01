using Application.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Shared.DTO;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;

namespace FleetGuardAPI.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        //private const string API_KEY_HEADER = "X-API-Key";
        //private readonly string _configuredApiKey;
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
           // _configuredApiKey = configuration?["FleetGuard:ApiKey"]??""; 

        }

        public async Task Invoke(HttpContext context)
        {

            try
            {
                //validate for Unauthorized
                //if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var extractedApiKey))
                //{
                //    context.Response.StatusCode = 401; // Unauthorized
                //    await context.Response.WriteAsync("API Key was not provided.");
                //    return;
                //}

                //if (!_configuredApiKey.Equals(extractedApiKey))
                //{
                //    context.Response.StatusCode = 403; // Forbidden
                //    await context.Response.WriteAsync("Unauthorized client.");
                //    return;
                //}

                await _next(context); // Continue to next middleware
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var errorResponse = new
                {
                    context.Response.StatusCode,
                    Message = "An unexpected error occurred. Please try again later.",
                    Details = ex.Message  // Optional: Remove in production if exposing internals is a concern
                };

                var json = JsonSerializer.Serialize(errorResponse);

                await context.Response.WriteAsync(json);
            }
        }
    }

    
}
