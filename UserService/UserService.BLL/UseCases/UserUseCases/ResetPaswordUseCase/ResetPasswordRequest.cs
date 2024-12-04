using MediatR;

namespace UserService.BLL.UseCases.UserUseCases.ResetPaswordUseCase;

public sealed record ResetPasswordRequest(string Password, string Code) : IRequest<string> { }
