using AutoMapper;
using SupportService.Application.DTO;
using SupportService.Domain.Entities;

namespace SupportService.Application.Mapping;

internal class MessageDtoMappingProfile : Profile
{
    public MessageDtoMappingProfile()
    {
        CreateMap<Message, MessageDTO>()
            .ForMember(dest => dest.ChatOwnerId, opt => opt.MapFrom(src => src.Chat.ClientId));
    }
}
