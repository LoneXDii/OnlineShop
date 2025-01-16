using AutoMapper;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using SupportService.Application.DTO;
using SupportService.Application.Exceptions;
using SupportService.Domain.Abstractions;

namespace SupportService.Application.UseCases.CloseChat;

internal class CloseChatRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CloseChatRequestHandler> logger)
    : IRequestHandler<CloseChatRequest, ChatDTO>
{
    public async Task<ChatDTO> Handle(CloseChatRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"User with id: {request.UserId} trying to close chat with id: {request.ChatId}");

        var chat = await unitOfWork.ChatRepository.GetByIdAsync(request.ChatId, cancellationToken);

        if (chat is null)
        {
            logger.LogError($"Chat with id: {request.ChatId} not found");

            throw new BadRequestException("No such chat");
        }

        if (request.UserId is not null && chat.ClientId != request.UserId)
        {
            logger.LogError($"User with id:{request.UserId} has no access to chat with id: {request.ChatId}");

            throw new BadRequestException("You have no access to this chat");
        }

        chat.IsActive = false;

        await unitOfWork.ChatRepository.UpdateAsync(chat, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);

        BackgroundJob.Schedule(() => unitOfWork.ChatRepository.DeleteAsync(chat, default), TimeSpan.FromDays(7));

        logger.LogInformation($"User with id:{request.UserId} successfully closed chat with id: {request.ChatId}");

        return mapper.Map<ChatDTO>(chat);
    }
}
