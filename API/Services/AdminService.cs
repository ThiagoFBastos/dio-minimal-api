using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain;
using API.Domain.Exceptions;
using API.Domain.Entities;
using API.Domain.Repositories;
using API.Services.Contracts;
using API.Shared;
using AutoMapper;

namespace API.Services
{
    public class AdminService: IAdminService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public AdminService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<int> CreateAdmin(AdminForCreateDto admin)
        {
            Admin a = _mapper.Map<Admin>(admin);
            _repositoryManager.AdminRepository.CreateAdmin(a);
            await _repositoryManager.SaveChangesAsync();
            return a.Id;
        }
        public async Task<AdminDto> UpdateAdmin(int id, AdminForUpdateDto admin)
        {
            Admin? a = await _repositoryManager.AdminRepository.GetAdminByIdAsync(id);

            if(a is null)
                throw new NotFoundException();

            _mapper.Map(admin, a, typeof(AdminForUpdateDto), typeof(Admin));
            _repositoryManager.AdminRepository.UpdateAdmin(a);
            await _repositoryManager.SaveChangesAsync();
            return _mapper.Map<AdminDto>(a);
        }
        public async Task DeleteAdmin(int id)
        {
            Admin? a = await _repositoryManager.AdminRepository.GetAdminByIdAsync(id);

            if(a is null)
                throw new NotFoundException();

            _repositoryManager.AdminRepository.DeleteAdmin(a);
            await _repositoryManager.SaveChangesAsync();
        }
        public async Task<AdminDto?> GetAdmin(int id)
        {
            Admin? admin = await _repositoryManager.AdminRepository.GetAdminByIdAsync(id);
            return _mapper.Map<AdminDto?>(admin);
        }
        public async Task<List<AdminDto>> GetAllAdmins()
        {
            List<Admin> admins = await _repositoryManager.AdminRepository.GetAllAdminsAsync();
            return _mapper.Map<List<AdminDto>>(admins);
        }
        public async Task<AdminDto?> LoginAdmin(LoginDto login)
        {
            Admin? admin = await _repositoryManager.AdminRepository.GetAdminWithCredentials(login.Email, login.Password);
            return _mapper.Map<AdminDto>(admin);
        }
    }
}