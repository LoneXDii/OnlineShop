using MediatR;
using UserService.BLL.DTO;

namespace UserService.BLL.UseCases.AuthUseCases.RegisterUserUseCase;

public sealed record RegisterUserRequest(RegisterDTO RegisterModel) : IRequest { }