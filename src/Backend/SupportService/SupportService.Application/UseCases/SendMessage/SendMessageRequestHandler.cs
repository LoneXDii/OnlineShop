using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SupportService.Application.DTO;
using SupportService.Application.Exceptions;
using SupportService.Domain.Abstractions;
using SupportService.Domain.Entities;

namespace SupportService.Application.UseCases.SendMessage;

internal class SendMessageRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SendMessageRequestHandler> logger)
    : IRequestHandler<SendMessageRequest, MessageDTO>
{
    public async Task<MessageDTO> Handle(SendMessageRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"User with id: {request.UserId} sending message to chat with id: {request.Message.ChatId}");

        var chat = await unitOfWork.ChatRepository.GetByIdAsync(request.Message.ChatId, cancellationToken);

        if(chat is null)
        {
            logger.LogError($"Chat with id: {request.Message.ChatId} not found");

            throw new BadRequestException("No such chat"); 
        }

        if (!chat.IsActive)
        {
            logger.LogError($"Chat with id: {request.Message.ChatId} is closed");

            throw new BadRequestException("This chat is closed");
        }

        var message = mapper.Map<Message>(request);

        await unitOfWork.MessageRepository.AddAsync(message, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);

        return mapper.Map<MessageDTO>(message);
    }
}
