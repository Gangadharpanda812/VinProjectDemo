using Application.Interface;
using Domain;
using Infrastructure.Repositories;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{

    internal class VehicleServices(IGenericRepository<Vehicle> vehicleRepository) : IVehicleServices
    {
        private readonly IGenericRepository<Vehicle> _vehicleRepository = vehicleRepository;

        public async Task<VehicleDto> GetVehicleByVin(string vin)
        {
            var output = await _vehicleRepository.GetSingleOrDefaultAsync(r=> r.Vin == vin);
            if (output != null)
            {
                return new VehicleDto
                {
                    Vin = output.Vin,
                    EmissionsStatus = output.EmissionsStatus,
                    Make = output.Make,
                    Model = output.Model,
                    RecallStatus = output.RecallStatus,
                    RegistrationStatus = output.RegistrationStatus,
                    TitleState = output.TitleState,
                    Year = output.Year
                };
            }
            else
               return null;
        }
    }
}
