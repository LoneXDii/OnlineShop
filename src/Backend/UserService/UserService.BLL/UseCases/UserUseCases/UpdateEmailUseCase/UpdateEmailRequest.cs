using MediatR;

namespace UserService.BLL.UseCases.UserUseCases.UpdateEmailUseCase;

public sealed record UpdateEmailRequest(string newEmail, string userId) : IRequest { }