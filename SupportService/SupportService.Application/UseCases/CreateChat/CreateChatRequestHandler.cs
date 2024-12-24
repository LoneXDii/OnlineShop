using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SupportService.Application.DTO;
using SupportService.Application.Exceptions;
using SupportService.Domain.Abstractions;
using SupportService.Domain.Entities;

namespace SupportService.Application.UseCases.CreateChat;

internal class CreateChatRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateChatRequestHandler> logger)
    : IRequestHandler<CreateChatRequest, ChatDTO>
{
    public async Task<ChatDTO> Handle(CreateChatRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"User with id: {request.ClientId} trying to create chat");

        var dbChat = await unitOfWork.ChatRepository.FirstOrDefaultAsync(chat => 
            chat.ClientId == request.ClientId && chat.IsActive, cancellationToken);

        if(dbChat is not null)
        {
            logger.LogInformation($"User with id: {request.ClientId} already have opened chat");

            throw new BadRequestException("This chat is already exists");
        }

        var chat = mapper.Map<Chat>(request);
        
        await unitOfWork.ChatRepository.AddAsync(chat, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);

        logger.LogInformation($"User with id: {request.ClientId} successfully created chat with id: {chat.Id}");

        return mapper.Map<ChatDTO>(chat);
    }
}
