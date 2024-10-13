using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domain.Repositories
{
    public interface IRepositoryManager
    {
        IAdminRepository AdminRepository { get; }
        IVehicleRepository VehicleRepository { get; }
        Task SaveChangesAsync();
    }
}