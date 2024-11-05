using AutoMapper;
using UserService.Application.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Mapping;

internal class AppMappingProfile : Profile
{
	public AppMappingProfile()
	{
		CreateMap<RegisterModel, AppUser>()
			.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
	}
}
