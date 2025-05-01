using Application.Interface;
using Domain;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{

    internal class ReportService(IGenericRepository<ApiLogs> ApilogsRepository, IGenericRepository<Vehicle> ApiVehicleRepository) : IReportService
    {

        public async Task<IEnumerable<TopVinDto>> GetTop5VehiclesByVin()
        {
            var topVinStats = await (
                 from log in ApilogsRepository.Table
                 let vin1 = log.RequestPath.Replace("/api/v1/vehicle/", "")
                 group vin1 by vin1 into g
                 orderby g.Count() descending
                 select new
                 {
                     Vin = g.Key,
                     RecordCount = g.Count()
                 })
                 .Take(5)
                 .Join(
                     ApiVehicleRepository.Table,
                     vinGroup => vinGroup.Vin,
                     vehicle => vehicle.Vin,
                     (vinGroup, vehicle) => new TopVinDto
                     {
                         RecordCount = vinGroup.RecordCount,
                         Vin = vinGroup.Vin,
                         VehicleName = vehicle.Year + " " + vehicle.Make + " " + vehicle.Model
                     })
                 .OrderByDescending(x => x.RecordCount)
                 .ToListAsync();

            return topVinStats;
        }

        public async Task<IEnumerable<HourlyLogCountDto>> GetHourlyLogCountinfo()
        {
            var result = await ApilogsRepository.Table
            .Where(a => a.StartTime.Date == DateTime.UtcNow.Date) // Filter by today's date
            .GroupBy(a => a.StartTime.Hour) // Group by the hour part of the start_time
            .Select(g => new HourlyLogCountDto
            {
                Hour = g.Key,
                RecordCount = g.Count()
            })
            .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<AnalyticsStatsDto>> GetAnalyticSummaryinfo()
        {
            var baseData = await ApilogsRepository.Table
             .Where(a => a.StartTime.Date == DateTime.UtcNow.Date || a.StartTime.Date == DateTime.UtcNow.AddDays(-1).Date)
             .Select(a => new
             {
                 RequestDate = a.StartTime.Date,
                 Vin = a.RequestPath.Replace("/api/v1/vehicle/", ""),
                 a.Duration
             })
             .ToListAsync();

            var aggregated = baseData
                .GroupBy(b => b.RequestDate)
                .Select(g => new
                {
                    RequestDate = g.Key,
                    RequestCount = g.Count(),
                    TotalResponseTime = Math.Round(g.Average(b => Convert.ToDouble(b.Duration??0))) // Round average duration
                })
                .ToList();

            var uniqueVinCount = baseData
                .GroupBy(b => b.RequestDate)
                .Select(g => new
                {
                    RequestDate = g.Key,
                    UniqueVinCount = g.Select(b => b.Vin).Distinct().Count()
                })
                .ToList();

                // Join aggregated data with uniqueVinCount data
                var result = from agg in aggregated
                             join vinCount in uniqueVinCount
                             on agg.RequestDate equals vinCount.RequestDate
                             orderby agg.RequestDate descending
                             select new AnalyticsStatsDto
                             {
                                 RequestDate =  agg.RequestDate,
                                  RequestCount = agg.RequestCount,
                                  AvgResponseTime = agg.TotalResponseTime,
                                  UniqueVinCount = vinCount.UniqueVinCount
                             };

                return result;
        }



    }
}
