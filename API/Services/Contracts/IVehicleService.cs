using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Shared;

namespace API.Services.Contracts
{
    public interface IVehicleService
    {
        Task<int> CreateVehicle(VehicleForCreateDto vehicle);
        Task<VehicleDto> UpdateVehicle(int id, VehicleForUpdateDto vehicle);
        Task DeleteVehicle(int id);
        Task<VehicleDto?> GetVehicle(int id);
        Task<List<VehicleDto>> GetAllVehicles();
    }
}