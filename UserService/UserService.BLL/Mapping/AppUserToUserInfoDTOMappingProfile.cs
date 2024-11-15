using AutoMapper;
using UserService.BLL.DTO;
using UserService.DAL.Entities;

namespace UserService.BLL.Mapping;

internal class AppUserToUserInfoDTOMappingProfile : Profile
{
    public AppUserToUserInfoDTOMappingProfile()
    {
        CreateMap<AppUser, UserInfoDTO>()
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.AvatarUrl));
    }
}
