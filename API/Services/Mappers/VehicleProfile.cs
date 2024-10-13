using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain.Entities;
using API.Shared;
using AutoMapper;

namespace API.Services.Mappers
{
    public class VehicleProfile: Profile
    {
        public VehicleProfile()
        {
            CreateMap<VehicleForCreateDto, Vehicle>()
                .ForMember(v => v.Id, opt => opt.Ignore());

            CreateMap<VehicleForUpdateDto, Vehicle>()
                .ForMember(v => v.Id, opt => opt.Ignore());

            CreateMap<Vehicle, VehicleDto>();
        }
    }
}