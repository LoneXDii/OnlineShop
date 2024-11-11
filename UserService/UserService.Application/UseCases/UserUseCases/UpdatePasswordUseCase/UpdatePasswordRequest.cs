using MediatR;
using UserService.Application.DTO;

namespace UserService.Application.UseCases.UserUseCases.UpdatePasswordUseCase;

public sealed record UpdatePasswordRequest(UpdatePasswordDTO updatePasswordDTO, string userId) : IRequest<string> { }