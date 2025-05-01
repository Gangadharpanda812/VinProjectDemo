using Application.Interface;
using Application.Services;
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
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IVehicleServices, VehicleServices>();
            services.AddScoped<IApilogsServices, ApilogsServices>();
            services.AddScoped<IReportService, ReportService>();
            // Add other services here...

            return services;
        }
    }
}
