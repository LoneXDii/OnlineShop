using MediatR;
using UserService.Application.Models;

namespace UserService.Application.UseCases.AuthUseCases.RegisterUserUseCase;

public sealed record RegisterUserRequest(RegisterModel RegisterModel) : IRequest { }