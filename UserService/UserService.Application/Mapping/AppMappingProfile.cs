using AutoMapper;
using UserService.Application.DTO;
using UserService.Domain.Entities;

namespace UserService.Application.Mapping;

internal class AppMappingProfile : Profile
{
	public AppMappingProfile()
	{
		CreateMap<RegisterDTO, AppUser>()
			.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

		CreateMap<AppUser, UserInfoDTO>();

		CreateMap<UpdateUserDTO, AppUser>();
	}
}
