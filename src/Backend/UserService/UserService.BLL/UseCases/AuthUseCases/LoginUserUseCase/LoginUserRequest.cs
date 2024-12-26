using MediatR;
using UserService.BLL.DTO;
using UserService.DAL.Models;

namespace UserService.BLL.UseCases.AuthUseCases.LoginUserUseCase;

public sealed record LoginUserRequest(LoginDTO LoginModel) : IRequest<TokensDTO> { }