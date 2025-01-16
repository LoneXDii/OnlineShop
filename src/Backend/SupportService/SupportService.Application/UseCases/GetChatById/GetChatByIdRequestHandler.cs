using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SupportService.Application.DTO;
using SupportService.Application.Exceptions;
using SupportService.Domain.Abstractions;

namespace SupportService.Application.UseCases.GetChatById;

internal class GetChatByIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetChatByIdRequestHandler> logger)
    : IRequestHandler<GetChatByIdRequest, ChatDTO>
{
    public async Task<ChatDTO> Handle(GetChatByIdRequest request, CancellationToken cancellationToken)
    {
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

        return mapper.Map<ChatDTO>(chat);
    }
}
