using AutoMapper;
using MediatR;
using SupportService.Application.DTO;
using SupportService.Domain.Abstractions;
using SupportService.Domain.Entities;

namespace SupportService.Application.UseCases.SendMessage;

internal class SendMessageRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<SendMessageRequest, MessageDTO>
{
    public async Task<MessageDTO> Handle(SendMessageRequest request, CancellationToken cancellationToken)
    {
        var message = mapper.Map<Message>(request);

        message = await unitOfWork.MessageRepository.AddAsync(message, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);

        var messageDto = mapper.Map<MessageDTO>(message);

        return messageDto;
    }
}
