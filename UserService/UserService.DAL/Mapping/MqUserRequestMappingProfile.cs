using AutoMapper;

using UserService.DAL.Entities;
using UserService.DAL.Models;

namespace UserService.DAL.Mapping;

internal class MqUserRequestMappingProfile : Profile
{
    public MqUserRequestMappingProfile()
    {
        CreateMap<AppUser, MqUserRequest>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
    }
}
