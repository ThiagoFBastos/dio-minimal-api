using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain.Entities;
using API.Shared;

namespace API.Services.Contracts
{
    public interface IAdminService
    {
        Task<int> CreateAdmin(AdminForCreateDto admin);
        Task<AdminDto> UpdateAdmin(int id, AdminForUpdateDto admin);
        Task DeleteAdmin(int id);
        Task<AdminDto?> GetAdmin(int id);
        Task<List<AdminDto>> GetAllAdmins();
        Task<AdminDto?> LoginAdmin(LoginDto login);
    }
}