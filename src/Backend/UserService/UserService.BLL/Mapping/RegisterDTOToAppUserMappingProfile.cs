using AutoMapper;
using UserService.BLL.DTO;
using UserService.DAL.Entities;

namespace UserService.BLL.Mapping;

internal class RegisterDTOToAppUserMappingProfile : Profile
{
    public RegisterDTOToAppUserMappingProfile()
    {
        CreateMap<RegisterDTO, AppUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
    }
}