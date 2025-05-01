using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
     
    public record TopVinDto
    {
        public string Vin { get; init; }
        public int RecordCount { get; init; }
        public string VehicleName { get; init; }
    }

    public record HourlyLogCountDto
    {
        public int Hour { get; init; }
        public int RecordCount { get; init; }

    }

    public record AnalyticsStatsDto
    {
        public DateTime RequestDate { get; init; }
        public int RequestCount { get; init; }
        public double AvgResponseTime { get; init; }  // Adjust type if needed
        public int UniqueVinCount { get; init; }
    }
}
