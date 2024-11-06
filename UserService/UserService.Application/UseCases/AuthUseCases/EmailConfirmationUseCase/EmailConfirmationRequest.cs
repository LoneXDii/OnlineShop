using MediatR;

namespace UserService.Application.UseCases.AuthUseCases.EmailConfirmationUseCase;

public sealed record EmailConfirmationRequest(string email, string code) : IRequest { }