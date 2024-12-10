using MediatR;
using SupportService.Application.DTO;

namespace SupportService.Application.UseCases.CreateChat;

public sealed record CreateChatRequest(string ClientId, string ClientName) : IRequest<ChatDTO> { }
