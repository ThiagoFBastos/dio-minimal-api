using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain.Entities;
using API.Domain.Repositories;
using API.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Repositories
{
    public class VehicleRepository: RepositoryBase<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(DataContext context): base(context) {}
        public void CreateVehicle(Vehicle vehicle) => Add(vehicle);
        public void UpdateVehicle(Vehicle vehicle) => Update(vehicle);
        public void DeleteVehicle(Vehicle vehicle) => Delete(vehicle);
        public Task<Vehicle?> GetVehicleAsync(int id) => FindByCondition(v => v.Id == id).FirstOrDefaultAsync();
        public Task<List<Vehicle>> GetAllVehiclesAsync() => FindAll().ToListAsync();
    }
}