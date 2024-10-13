using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain;
using API.Domain.Entities;
using API.Domain.Exceptions;
using API.Domain.Repositories;
using API.Services.Contracts;
using API.Shared;
using AutoMapper;

namespace API.Services
{
    public class VehicleService: IVehicleService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        public VehicleService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }
        public async Task<int> CreateVehicle(VehicleForCreateDto vehicle)
        {
            Vehicle v = _mapper.Map<Vehicle>(vehicle);
            _repositoryManager.VehicleRepository.CreateVehicle(v);
            await _repositoryManager.SaveChangesAsync();
            return v.Id;
        }
        public async Task<VehicleDto> UpdateVehicle(int id, VehicleForUpdateDto vehicle)
        {
            Vehicle? v = await _repositoryManager.VehicleRepository.GetVehicleAsync(id);

            if(v is null)
                throw new NotFoundException(); 

            _mapper.Map(vehicle, v, typeof(VehicleForUpdateDto), typeof(Vehicle));
            _repositoryManager.VehicleRepository.UpdateVehicle(v);
            await _repositoryManager.SaveChangesAsync();

            return _mapper.Map<VehicleDto>(v);
        }
        public async Task DeleteVehicle(int id)
        {
            Vehicle? vehicle = await _repositoryManager.VehicleRepository.GetVehicleAsync(id);

            if(vehicle is null)
                throw new NotFoundException();

            _repositoryManager.VehicleRepository.DeleteVehicle(vehicle);
            await _repositoryManager.SaveChangesAsync();
        }
        public async Task<VehicleDto?> GetVehicle(int id)
        {
            Vehicle? vehicle = await _repositoryManager.VehicleRepository.GetVehicleAsync(id);
            return _mapper.Map<VehicleDto?>(vehicle);
        }
        public async Task<List<VehicleDto>> GetAllVehicles()
        {
            List<Vehicle> vehicles = await _repositoryManager.VehicleRepository.GetAllVehiclesAsync();
            return _mapper.Map<List<VehicleDto>>(vehicles);
        }
    }
}