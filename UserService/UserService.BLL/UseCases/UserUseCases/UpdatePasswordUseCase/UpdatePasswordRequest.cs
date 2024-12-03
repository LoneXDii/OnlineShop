using MediatR;
using UserService.BLL.DTO;

namespace UserService.BLL.UseCases.UserUseCases.UpdatePasswordUseCase;

public sealed record UpdatePasswordRequest(UpdatePasswordDTO updatePasswordDTO, string userId) : IRequest<string> { }