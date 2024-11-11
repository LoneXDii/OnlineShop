using MediatR;
using UserService.Application.DTO;

namespace UserService.Application.UseCases.UserUseCases.GetUserInfoUseCase;

public sealed record GetUserInfoRequest(string userId) : IRequest<UserInfoDTO> { }