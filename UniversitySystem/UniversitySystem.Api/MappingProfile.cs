using AutoMapper;
using UniversitySystem.Api.Models;
using UniversitySystem.Data.Entities;
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
            CreateMap<EditUserModel, EditUserDto>();
            CreateMap<EditUserDto, User>();
            CreateMap<User, MainUserInfoDto>();
            CreateMap<User, UserForAdminPanelDto>();
            CreateMap<User, UserDto>();
            CreateMap<Role, RoleDto>()
                .ForMember(m => m.Users, 
                    x => x.Ignore());
            CreateMap<NewRoleModel, NewRoleDto>();
            CreateMap<EditRoleModel, EditRoleDto>();
            CreateMap<EditRoleDto, Role>();
            CreateMap<ReloginModel, ReloginDto>();
            CreateMap<NewStudentModel, NewStudentDto>();
            CreateMap<NewStudentDto, Student>();
            CreateMap<NewTeacherModel, NewTeacherDto>();
            CreateMap<NewTeacherDto, Teacher>();
            CreateMap<NewLessonModel, NewLessonDTO>();
            CreateMap<EditLessonModel, EditLessonDTO>();
            CreateMap<Lesson, CompactLessonDTO>()
                .ForMember(dto => dto.TeacherName,
                    opt => opt.MapFrom(
                        l => l.Teacher.User.FullName));
            CreateMap<Lesson, LessonDTO>()
                .ForMember(dto => dto.TeacherName,
                    opt => opt.MapFrom(
                        l => l.Teacher.User.FullName));
        }
    }
}
