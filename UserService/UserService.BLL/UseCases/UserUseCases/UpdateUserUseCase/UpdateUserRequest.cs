using MediatR;
using UserService.BLL.DTO;

namespace UserService.BLL.UseCases.UserUseCases.UpdateUserUseCase;

public sealed record UpdateUserRequest(UpdateUserDTO userDTO, string userId) : IRequest { }