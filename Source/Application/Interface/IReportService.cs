using Shared.DTO;

namespace Application.Interface
{
    public interface IReportService
    {
        Task<IEnumerable<TopVinDto>> GetTop5VehiclesByVin();
        Task<IEnumerable<HourlyLogCountDto>> GetHourlyLogCountinfo();
        Task<IEnumerable<AnalyticsStatsDto>> GetAnalyticSummaryinfo();
    }
}