using AutoMapper;
using SupportService.Application.DTO;
using SupportService.Domain.Entities;

namespace SupportService.Application.Mapping;

internal class ChatDtoMappingProfile : Profile
{
    public ChatDtoMappingProfile()
    {
        CreateMap<Chat, ChatDTO>();
    }
}
