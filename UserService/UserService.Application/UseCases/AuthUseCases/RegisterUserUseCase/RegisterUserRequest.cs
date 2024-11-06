using MediatR;
using UserService.Application.DTO;

namespace UserService.Application.UseCases.AuthUseCases.RegisterUserUseCase;

public sealed record RegisterUserRequest(RegisterDTO RegisterModel) : IRequest { }