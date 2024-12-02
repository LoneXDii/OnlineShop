using AutoMapper;
using MediatR;
using SupportService.Application.DTO;
using SupportService.Application.Exceptions;
using SupportService.Domain.Abstractions;
using SupportService.Domain.Entities;

namespace SupportService.Application.UseCases.CreateChat;

internal class CreateChatRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateChatRequest, ChatDTO>
{
    public async Task<ChatDTO> Handle(CreateChatRequest request, CancellationToken cancellationToken)
    {
        var dbChat = await unitOfWork.ChatRepository.FirstOrDefaultAsync(chat => 
            chat.ClientId == request.ClientId && chat.IsActive, cancellationToken);

        if(dbChat is not null)
        {
            throw new BadRequestException("This chat is already exists");
        }

        var chat = mapper.Map<Chat>(request);
        
        chat = await unitOfWork.ChatRepository.AddAsync(chat, cancellationToken);

        var chatDto = mapper.Map<ChatDTO>(chat);

        await unitOfWork.SaveAllAsync(cancellationToken);

        return chatDto;
    }
}
