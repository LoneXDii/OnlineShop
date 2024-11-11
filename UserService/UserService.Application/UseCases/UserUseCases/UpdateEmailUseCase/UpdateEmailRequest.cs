using MediatR;

namespace UserService.Application.UseCases.UserUseCases.UpdateEmailUseCase;

public sealed record UpdateEmailRequest(string newEmail, string userId) : IRequest { }