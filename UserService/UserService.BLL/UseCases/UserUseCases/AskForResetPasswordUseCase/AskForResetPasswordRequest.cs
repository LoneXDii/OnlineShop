using MediatR;

namespace UserService.BLL.UseCases.UserUseCases.AskForResetPasswordUseCase;

public sealed record AskForResetPasswordRequest(string Email) : IRequest { }
