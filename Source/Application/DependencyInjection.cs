using Application.Interface;
using Application.Services;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
  

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddInfrastructureServices(configuration);
            // Add services to the container.

            services.AddScoped<IVehicleServices, VehicleServices>();
            services.AddScoped<IApilogsServices, ApilogsServices>();
            services.AddScoped<IReportService, ReportService>();
            // Add other services here...

            return services;
        }
    }
}
