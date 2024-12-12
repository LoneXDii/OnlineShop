using AutoMapper;
using MediatR;
using SupportService.Application.DTO;
using SupportService.Application.Exceptions;
using SupportService.Domain.Abstractions;

namespace SupportService.Application.UseCases.CloseChat;

internal class CloseChatRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CloseChatRequest, ChatDTO>
{
    public async Task<ChatDTO> Handle(CloseChatRequest request, CancellationToken cancellationToken)
    {
        var chat = await unitOfWork.ChatRepository.GetByIdAsync(request.ChatId, cancellationToken);

        if (chat is null)
        {
            throw new BadRequestException("No such chat");
        }

        if (request.UserId is not null)
        {
            if (chat.ClientId != request.UserId)
            {
                throw new BadRequestException("You have no access to this chat");
            }
        }

        chat.IsActive = false;

        await unitOfWork.ChatRepository.UpdateAsync(chat, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);

        return mapper.Map<ChatDTO>(chat);
    }
}
