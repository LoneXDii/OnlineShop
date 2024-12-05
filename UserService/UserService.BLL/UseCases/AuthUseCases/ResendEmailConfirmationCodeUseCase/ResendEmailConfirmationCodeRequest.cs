using MediatR;

namespace UserService.BLL.UseCases.AuthUseCases.ResendEmailConfirmationCodeUseCase;

public sealed record ResendEmailConfirmationCodeRequest(string Email) : IRequest { }
