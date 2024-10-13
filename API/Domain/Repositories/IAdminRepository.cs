using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain.Entities;

namespace API.Domain.Repositories
{
    public interface IAdminRepository
    {
        void CreateAdmin(Admin admin);
        void UpdateAdmin(Admin admin);
        void DeleteAdmin(Admin admin);
        Task<Admin?> GetAdminByIdAsync(int id);
        Task<List<Admin>> GetAllAdminsAsync();
        Task<Admin?> GetAdminWithCredentials(string email, string password);
    }
}