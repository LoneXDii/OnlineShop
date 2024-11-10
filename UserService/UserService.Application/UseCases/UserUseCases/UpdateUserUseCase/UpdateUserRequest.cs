using MediatR;
using UserService.Application.DTO;

namespace UserService.Application.UseCases.UserUseCases.UpdateUserUseCase;

public sealed record UpdateUserRequest(UpdateUserDTO userDTO, string userId) : IRequest { }