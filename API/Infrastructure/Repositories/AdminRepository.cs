using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Threading.Tasks;
using API.Domain.Entities;
using API.Domain.Repositories;
using API.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Repositories
{
    public class AdminRepository: RepositoryBase<Admin>, IAdminRepository
    {
        public AdminRepository(DataContext context): base(context)
        {

        }
        public void CreateAdmin(Admin admin) => Add(admin);
        public void UpdateAdmin(Admin admin) => Update(admin);
        public void DeleteAdmin(Admin admin) => Delete(admin);
        public Task<Admin?> GetAdminByIdAsync(int id) => FindByCondition(a => a.Id == id).FirstOrDefaultAsync();
        public Task<List<Admin>> GetAllAdminsAsync() => FindAll().ToListAsync();
        public Task<Admin?> GetAdminWithCredentials(string email, string password) => FindByCondition(a => a.Email == email && a.Password == password).FirstOrDefaultAsync();
    }
}