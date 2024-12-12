using AutoMapper;
using MediatR;
using SupportService.Application.DTO;
using SupportService.Domain.Abstractions;

namespace SupportService.Application.UseCases.GetAllChats;

internal class GetAllChatsRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllChatsRequest, List<ChatDTO>>
{
    public async Task<List<ChatDTO>> Handle(GetAllChatsRequest request, CancellationToken cancellationToken)
    {
        var chats = await unitOfWork.ChatRepository.ListAllAsync(cancellationToken);

        return mapper.Map<List<ChatDTO>>(chats);
    }
}
