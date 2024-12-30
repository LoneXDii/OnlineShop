using MediatR;
using SupportService.Application.DTO;

namespace SupportService.Application.UseCases.CloseChat;

public sealed record CloseChatRequest(int ChatId, string? UserId = null) : IRequest<ChatDTO> { }
