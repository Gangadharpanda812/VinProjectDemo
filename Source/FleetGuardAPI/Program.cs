
using Application;
using Application.Interface;
using AspNetCoreRateLimit;
using FleetGuardAPI.Middleware;
using FleetGuardAPI.Models;
using Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Shared.DTO;
using System.Globalization;
namespace FleetGuardAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddApplicationServices(builder.Configuration);
           

            #region Logging with serilog

            //var week = ISOWeek.GetWeekOfYear(DateTime.UtcNow);
            //var year = DateTime.UtcNow.Year;
            //var logFile = $"Logs/log-{year}-W{week}.json";

            //Log.Logger = new LoggerConfiguration()
            //  .Enrich.FromLogContext()  // Retain contextual information
            //  .WriteTo.Console()  // Log to the console
            //  .WriteTo.File(
            //      new Serilog.Formatting.Json.JsonFormatter(renderMessage: true),
            //      logFile  // You can change this path
            //  )
            //  .CreateLogger();
            #endregion


            #region Configure rate limiting options
           
                builder.Services.AddLogging();
                builder.Services.AddOptions();
                builder.Services.AddMemoryCache();
                builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
                builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimiting"));
                builder.Services.AddInMemoryRateLimiting();
                builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            #endregion


            #region outputcache

            builder.Services.AddResponseCaching();

            #endregion
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddResponseCompression();

            //builder.Host.UseSerilog();

            var app = builder.Build();

            app.UseIpRateLimiting();

            app.UseMiddleware<LoggingMiddleware>();

            app.MapGet("/api/v1/vehicle/{vin}", async (string vin, IVehicleServices vehicleServices, ILogger<Program> logger, HttpContext context) =>
            {
                if (string.IsNullOrWhiteSpace(vin))
                {
                    var errorResponse = new ApiError
                    {
                        Error = new ApiError.ErrorDetails
                        {
                            Code = "invalid_request",
                            Message = $"invalid request"
                        }
                    };
                    return Results.BadRequest(errorResponse);
                }

                if (vin.Length != 17)
                {
                    var errorResponse = new ApiError
                    {
                        Error = new ApiError.ErrorDetails
                        {
                            Code = "invalid_vin",
                            Message = $"Invalid Vin Length {vin.Length}. Please enter a valid 17-digit Vin."
                        }
                    };

                    return Results.UnprocessableEntity(errorResponse);
                }

                logger.LogInformation($"Received Vin: {vin}");

                var vehicle = await vehicleServices.GetVehicleByVin(vin);

                if (vehicle == null)
                {
                    var errorResponse = new ApiError
                    {
                        Error = new ApiError.ErrorDetails
                        {
                            Code = "not_found",
                            Message = $"Vehicle with VIN {vin} not found"
                        }
                    };

                    return Results.NotFound(errorResponse);
                }

                // Only set headers when returning 200
                context.Response.GetTypedHeaders().CacheControl = new()
                {
                    Public = true,
                    MaxAge = TimeSpan.FromMinutes(5)
                };

                context.Response.Headers["Vary"] = "Accept-Encoding";

                return Results.Ok(vehicle);
            })
            .WithName("GetVehicleByVin")
            .Produces<ApiError>(StatusCodes.Status400BadRequest) // Invalid Request
            .Produces<ApiError>(StatusCodes.Status422UnprocessableEntity) // Validation Error
            .Produces<ApiError>(StatusCodes.Status404NotFound) // Not Found
            .Produces<VehicleDto>(StatusCodes.Status200OK); // Successful Response;


            app.MapGet("/api/v1/Analytics/GetTop5Vininfo", async (IReportService reportService, ILogger<Program> logger) =>
            {
                var Result = await reportService.GetTop5VehiclesByVin();

                if (Result == null)
                {

                    var errorResponse = new ApiError
                    {
                        Error = new ApiError.ErrorDetails
                        {
                            Code = "not_found",
                            Message = $"No Record found."
                        }
                    };

                    return Results.NotFound(errorResponse);
                }

                return Results.Ok(Result);
            })
            .Produces<ApiError>(StatusCodes.Status404NotFound) // Not Found
            .Produces<TopVinDto>(StatusCodes.Status200OK); // Successful Response;;

            app.MapGet("/api/v1/Analytics/GetHourlyLogCountinfo", async (IReportService reportService, ILogger<Program> logger) =>
            {
                var Result = await reportService.GetHourlyLogCountinfo();

                if (Result == null)
                {

                    var errorResponse = new ApiError
                    {
                        Error = new ApiError.ErrorDetails
                        {
                            Code = "not_found",
                            Message = $"No Record found."
                        }
                    };

                    return Results.NotFound(errorResponse);
                }

                return Results.Ok(Result);
            })
            .Produces<ApiError>(StatusCodes.Status404NotFound) // Not Found
            .Produces<HourlyLogCountDto>(StatusCodes.Status200OK); // Successful Response
                                                           



            app.MapGet("/api/v1/Analytics/GetRequestSummaryinfo", async (IReportService reportService, ILogger<Program> logger) =>
            {
                var Result = await reportService.GetAnalyticSummaryinfo();

                if (Result == null)
                {

                    var errorResponse = new ApiError
                    {
                        Error = new ApiError.ErrorDetails
                        {
                            Code = "not_found",
                            Message = $"No Record found."
                        }
                    };

                    return Results.NotFound(errorResponse);
                }

                return Results.Ok(Result);
            })
            .Produces<ApiError>(StatusCodes.Status404NotFound) // Not Found
            .Produces<AnalyticsStatsDto>(StatusCodes.Status200OK); // Successful Response


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

           
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseResponseCompression();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
