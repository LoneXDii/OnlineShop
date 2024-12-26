using MediatR;
using SupportService.Application.DTO;

namespace SupportService.Application.UseCases.GetUserChats;

public sealed record GetUserChatsRequest(string UserId) : IRequest<List<ChatDTO>> { }
