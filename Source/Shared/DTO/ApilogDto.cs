namespace Shared.DTO;

public record ApiLogDto
{
    public long TransactionId { get; init; }
    public string LogType { get; init; }

    public DateTime StartTime { get; init; }

    public DateTime? EndTime { get; init; }

    public float? Duration { get; init; }

    public string RequestMethod { get; init; }

    public string RequestPath { get; init; }

    public int? ResponseStatusCode { get; init; }

}
public record InsertApiLogDto
{
    public string LogType { get; init; }

    public DateTime StartTime { get; init; }

    public DateTime? EndTime { get; init; }

    public float? Duration { get; init; }

    public string RequestMethod { get; init; }

    public string RequestPath { get; init; }

    public int? ResponseStatusCode { get; init; }

}

