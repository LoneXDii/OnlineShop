using MediatR;

namespace UserService.BLL.UseCases.UserUseCases.ResetPasswordUseCase;

public sealed record ResetPasswordRequest(string Password, string Code) : IRequest { }
