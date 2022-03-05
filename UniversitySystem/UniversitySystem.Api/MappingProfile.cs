using AutoMapper;
using UniversitySystem.Api.Models;
using UniversitySystem.Services.Dtos;

namespace UniversitySystem.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LoginModel, LoginDto>();
            CreateMap<RegisterModel, RegisterDto>();
            CreateMap<ChangePasswordModel, ChangePasswordDto>();
        }
    }
}
