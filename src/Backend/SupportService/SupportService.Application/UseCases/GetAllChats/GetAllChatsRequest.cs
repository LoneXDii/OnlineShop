using MediatR;
using SupportService.Application.DTO;

namespace SupportService.Application.UseCases.GetAllChats;

public sealed record GetAllChatsRequest() : IRequest<List<ChatDTO>> { }
