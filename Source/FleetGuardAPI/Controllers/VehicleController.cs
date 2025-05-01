using Microsoft.AspNetCore.Mvc;
using Shared.DTO;
using Application;
using Application.Interface;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Http.HttpResults;
namespace FleetGuardAPI.Controllers
{
    //[ApiController]
    //[Route("[controller]")]
    //public class VehicleController(ILogger<VehicleController> logger, IVehicleServices vehicleServices) : ControllerBase
    //{
    //    [EnableRateLimiting("FixedPolicy")]
    //    [HttpGet(Name = "api/v1/vehicle/{vin}")]
    //    public async Task<IAsyncResult> Get(string vin)
    //    {
    //        if (string.IsNullOrWhiteSpace(vin))
    //        {
    //            return (IAsyncResult)BadRequest("Invalid Vin");
    //        }
    //        if (string.IsNullOrEmpty(vin) && vin.Length != 17)
    //        {
    //            return (IAsyncResult)BadRequest($"Invalid Vin Lenth {vin.Length} Please enter valid 17 Digit Vin");
    //        }
    //        logger.LogInformation(message: $"Received Vin {vin}");
    //        return (IAsyncResult)await vehicleServices.GetVehicleByVin(vin);
    //    }
    //}
}
