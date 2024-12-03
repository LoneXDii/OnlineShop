using MediatR;

namespace UserService.BLL.UseCases.AuthUseCases.RefreshAccessTokenUseCase;

public sealed record RefreshAccessTokenRequest(string refreshToken) : IRequest<string> { }