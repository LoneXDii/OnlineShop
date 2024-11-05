using MediatR;
using UserService.Application.Models;
using UserService.Infrastructure.Models;

namespace UserService.Application.UseCases.AuthUseCases.LoginUserUseCase;

public sealed record LoginUserRequest(LoginModel LoginModel) : IRequest<TokensDTO> { }