using Application.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Shared.DTO;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FleetGuardAPI.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        //private readonly IApilogsServices _apilogsServices;
        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {

            var _apilogsServices = context.RequestServices.GetRequiredService<IApilogsServices>();
            var requestTime = DateTime.UtcNow;
            var stopwatch = Stopwatch.StartNew();

            // Enable buffering so the request body can be read multiple times
            context.Request.EnableBuffering();
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;

            // Swap the response stream to capture it
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context); // Proceed with the pipeline

            stopwatch.Stop();
            var responseTime = DateTime.UtcNow;

            // Read the response body
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            // Restore original response stream
            await responseBody.CopyToAsync(originalBodyStream);

            // Log everything in one go
            var logMessage = $@"[RequestResponseLog] 
                        Timestamp Start : {requestTime:O} Timestamp End   : {DateTime.UtcNow:O}
                        Duration        : {stopwatch.ElapsedMilliseconds} ms
                        Request         : {context.Request.Method} {context.Request.Path}
                        Request Body    : {requestBody}
                        Response Code   : {context.Response.StatusCode}
                        Response Body   : {responseText}
                        ";

            _logger.LogInformation(logMessage);
            if (context.Request.Path.ToString().Contains("/api/v1/vehicle"))
            {
                InsertApiLogDto insertApiLogDto = new InsertApiLogDto()
                {
                
                    StartTime = requestTime,
                    EndTime = DateTime.UtcNow,
                    Duration = stopwatch.ElapsedMilliseconds,
                    LogType = "RequestResponseLog",
                    RequestMethod = context.Request.Method,
                    RequestPath = context.Request.Path,
                    ResponseStatusCode = context.Response.StatusCode
                };
                var  ouput = await _apilogsServices.AddApilog(insertApiLogDto);
            }
        }
    }

    
}
