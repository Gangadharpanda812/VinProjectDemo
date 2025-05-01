using Shared.DTO;

namespace Application.Interface
{
    public interface IVehicleServices
    {
        Task<VehicleDto> GetVehicleByVin(string vin);
    }
}