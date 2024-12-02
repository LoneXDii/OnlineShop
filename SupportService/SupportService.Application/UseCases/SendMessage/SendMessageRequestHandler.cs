using AutoMapper;
using MediatR;
using SupportService.Application.DTO;
using SupportService.Application.Exceptions;
using SupportService.Domain.Abstractions;
using SupportService.Domain.Entities;

namespace SupportService.Application.UseCases.SendMessage;

internal class SendMessageRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<SendMessageRequest, MessageDTO>
{
    public async Task<MessageDTO> Handle(SendMessageRequest request, CancellationToken cancellationToken)
    {
        var chat = await unitOfWork.ChatRepository.GetByIdAsync(request.Message.ChatId, cancellationToken);

        if(chat is null)
        {
            throw new BadRequestException("No such chat"); 
        }

        if (!chat.IsActive)
        {
			throw new BadRequestException("This chat is closed");
		}

        var message = mapper.Map<Message>(request);

        await unitOfWork.MessageRepository.AddAsync(message, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);

        var messageDto = mapper.Map<MessageDTO>(message);

        return messageDto;
    }
}
