namespace Shared.DTO;
//output
public record VehicleDto
{
    public string Vin { get; init; } = string.Empty;
    public string Make { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public int? Year { get; init; } 
    public string TitleState { get; init; } = string.Empty;
    public string RegistrationStatus { get; init; } = string.Empty;
    public string EmissionsStatus { get; init; } = string.Empty;
    public string RecallStatus { get; init; } = string.Empty;
}
