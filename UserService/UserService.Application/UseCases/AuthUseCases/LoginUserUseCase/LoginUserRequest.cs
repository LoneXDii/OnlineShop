using MediatR;
using UserService.Application.DTO;
using UserService.Infrastructure.Models;

namespace UserService.Application.UseCases.AuthUseCases.LoginUserUseCase;

public sealed record LoginUserRequest(LoginDTO LoginModel) : IRequest<TokensDTO> { }