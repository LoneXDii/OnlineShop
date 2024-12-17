using MediatR;
using SupportService.Application.DTO;

namespace SupportService.Application.UseCases.SendMessage;

public sealed record SendMessageRequest(AddMessageDTO Message, string UserId) : IRequest<MessageDTO> { }
