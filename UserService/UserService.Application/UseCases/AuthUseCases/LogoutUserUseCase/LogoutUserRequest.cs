using MediatR;

namespace UserService.Application.UseCases.AuthUseCases.LogoutUserUseCase;

public sealed record LogoutUserRequest(string userId) : IRequest { }