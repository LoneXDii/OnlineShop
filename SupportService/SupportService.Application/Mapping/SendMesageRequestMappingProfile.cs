using AutoMapper;
using SupportService.Application.UseCases.SendMessage;
using SupportService.Domain.Entities;

namespace SupportService.Application.Mapping;

internal class SendMesageRequestMappingProfile : Profile
{
    SendMesageRequestMappingProfile()
    {
        CreateMap<SendMessageRequest, Message>()
            .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Message.Text))
            .ForMember(dest => dest.ChatId, opt => opt.MapFrom(src => src.Message.ChatId))
            .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}
