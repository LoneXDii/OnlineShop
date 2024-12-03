using AutoMapper;
using UserService.BLL.DTO;
using UserService.DAL.Entities;

namespace UserService.BLL.Mapping;

internal class UpdateUserDTOToAppUserMappingProfile : Profile
{
    public UpdateUserDTOToAppUserMappingProfile()
    {
        CreateMap<UpdateUserDTO, AppUser>();
    }
}
