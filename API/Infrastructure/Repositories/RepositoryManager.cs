using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain.Repositories;
using API.Infrastructure.Context;

namespace API.Infrastructure.Repositories
{
    public class RepositoryManager: IRepositoryManager
    {
        private readonly Lazy<IAdminRepository> _adminRepository;
        private readonly Lazy<IVehicleRepository> _vehicleRepository;
        private readonly DataContext _dataContext;
        public IAdminRepository AdminRepository => _adminRepository.Value;
        public IVehicleRepository VehicleRepository => _vehicleRepository.Value;

        public RepositoryManager(DataContext dataContext)
        {
            _dataContext = dataContext;
            _adminRepository = new Lazy<IAdminRepository>(() => new AdminRepository(dataContext));
            _vehicleRepository = new Lazy<IVehicleRepository>(() => new VehicleRepository(dataContext));
        }

        public Task SaveChangesAsync() => _dataContext.SaveChangesAsync();
    }
}