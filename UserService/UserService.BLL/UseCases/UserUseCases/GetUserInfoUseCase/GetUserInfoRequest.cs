using MediatR;
using UserService.BLL.DTO;

namespace UserService.BLL.UseCases.UserUseCases.GetUserInfoUseCase;

public sealed record GetUserInfoRequest(string userId) : IRequest<UserInfoDTO> { }