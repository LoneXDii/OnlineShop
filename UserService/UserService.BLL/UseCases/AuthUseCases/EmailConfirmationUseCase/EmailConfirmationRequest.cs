using MediatR;

namespace UserService.BLL.UseCases.AuthUseCases.EmailConfirmationUseCase;

public sealed record EmailConfirmationRequest(string email, string code) : IRequest { }