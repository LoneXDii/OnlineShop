using AutoMapper;
using MediatR;
using SupportService.Application.DTO;
using SupportService.Domain.Abstractions;

namespace SupportService.Application.UseCases.GetUserChats;

internal class GetUserChatsRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetUserChatsRequest, List<ChatDTO>>
{
    public async Task<List<ChatDTO>> Handle(GetUserChatsRequest request, CancellationToken cancellationToken)
    {
        var chats = await unitOfWork.ChatRepository.ListAsync(chat => chat.ClientId == request.UserId, cancellationToken);

        var chatsDto = mapper.Map<List<ChatDTO>>(chats);

        return chatsDto;
    }
}
