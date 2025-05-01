
using Application;
using Application.Interface;
using AspNetCoreRateLimit;
using FleetGuardAPI.Middleware;
using Infrastructure;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.InMemory;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System.Globalization;
namespace FleetGuardAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices();

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

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddResponseCompression();

            //builder.Host.UseSerilog();

            var app = builder.Build();

            app.UseIpRateLimiting();

            app.MapGet("/api/v1/vehicle/{vin}", async (string vin, IVehicleServices vehicleServices, ILogger<Program> logger) =>
            {
                if (string.IsNullOrWhiteSpace(vin))
                {
                    return Results.BadRequest("Invalid Vin");
                }

                if (vin.Length != 17)
                {
                    return Results.BadRequest($"Invalid Vin Length {vin.Length}. Please enter a valid 17-digit Vin.");
                }

                logger.LogInformation($"Received Vin: {vin}");

                var vehicle = await vehicleServices.GetVehicleByVin(vin);

                if (vehicle == null)
                {
                    return Results.NotFound($"Vehicle with VIN {vin} not found.");
                }

                return Results.Ok(vehicle);
            })
            .WithName("GetVehicleByVin");

            
            app.MapGet("/api/v1/Analytics/GetTop5Vininfo", async (IReportService reportService, ILogger<Program> logger) =>
            {
                var Result = await reportService.GetTop5VehiclesByVin();

                if (Result == null)
                {
                    return Results.NotFound($"No Record found.");
                }

                return Results.Ok(Result);
            });

            
            app.MapGet("/api/v1/Analytics/GetHourlyLogCountinfo", async (IReportService reportService, ILogger<Program> logger) =>
            {
                var Result = await reportService.GetHourlyLogCountinfo();

                if (Result == null)
                {
                    return Results.NotFound($"No Record found.");
                }

                return Results.Ok(Result);
            });

            
            app.MapGet("/api/v1/Analytics/GetRequestSummaryinfo", async (IReportService reportService, ILogger<Program> logger) =>
            {
                var Result = await reportService.GetAnalyticSummaryinfo();

                if (Result == null)
                {
                    return Results.NotFound($"No Record found.");
                }

                return Results.Ok(Result);
            });


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<LoggingMiddleware>();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseResponseCompression();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
