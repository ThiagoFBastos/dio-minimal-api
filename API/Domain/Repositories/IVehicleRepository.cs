using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain.Entities;

namespace API.Domain.Repositories
{
    public interface IVehicleRepository
    {
        void CreateVehicle(Vehicle vehicle);
        void UpdateVehicle(Vehicle vehicle);
        void DeleteVehicle(Vehicle vehicle);
        Task<Vehicle?> GetVehicleAsync(int id);
        Task<List<Vehicle>> GetAllVehiclesAsync();
    }
}