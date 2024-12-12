using AutoMapper;
using MediatR;
using SupportService.Application.DTO;
using SupportService.Domain.Abstractions;

namespace SupportService.Application.UseCases.GetChatMessages;

internal class GetChatMessagesRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetChatMessagesRequest, List<MessageDTO>>
{
    public async Task<List<MessageDTO>> Handle(GetChatMessagesRequest request, CancellationToken cancellationToken)
    {
        var messages = await unitOfWork.MessageRepository.ListAsync(message => message.ChatId == request.ChatId, cancellationToken);

        return mapper.Map<List<MessageDTO>>(messages);

    }
}
