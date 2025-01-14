using MediatR;
using SupportService.Application.DTO;

namespace SupportService.Application.UseCases.GetChatById;

public sealed record GetChatByIdRequest(int ChatId, string? UserId) : IRequest<ChatDTO> { }
