using AutoMapper;
using SupportService.Application.UseCases.CreateChat;
using SupportService.Domain.Entities;

namespace SupportService.Application.Mapping;

internal class CreateChatRequestMappingProfile : Profile
{
    public CreateChatRequestMappingProfile()
    {
        CreateMap<CreateChatRequest, Chat>();
    }
}
