using MediatR;
using SupportService.Application.DTO;

namespace SupportService.Application.UseCases.GetChatMessages;

public sealed record GetChatMessagesRequest(int ChatId) : IRequest<List<MessageDTO>> { }
