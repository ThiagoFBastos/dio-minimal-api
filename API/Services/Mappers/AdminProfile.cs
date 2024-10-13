using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using API.Domain.Entities;
using API.Shared;

namespace API.Services.Mappers
{
    public class AdminProfile: Profile
    {
        public AdminProfile()
        {
            CreateMap<AdminForCreateDto, Admin>()
                .ForMember(a => a.Id, opt => opt.Ignore());

            CreateMap<AdminForUpdateDto, Admin>()
                .ForMember(a => a.Id, opt => opt.Ignore());

            CreateMap<Admin, AdminDto>();
        }
    }
}