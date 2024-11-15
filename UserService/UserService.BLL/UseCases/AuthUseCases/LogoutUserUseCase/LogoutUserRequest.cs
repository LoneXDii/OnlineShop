using MediatR;

namespace UserService.BLL.UseCases.AuthUseCases.LogoutUserUseCase;

public sealed record LogoutUserRequest(string userId) : IRequest { }