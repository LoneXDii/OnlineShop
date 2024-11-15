using AutoMapper;
using UserService.BLL.DTO;
using UserService.DAL.Entities;

namespace UserService.BLL.Mapping;

internal class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<RegisterDTO, AppUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

        CreateMap<AppUser, UserInfoDTO>()
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.AvatarUrl));

        CreateMap<UpdateUserDTO, AppUser>();
    }
}
