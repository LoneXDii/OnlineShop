using MediatR;

namespace UserService.Application.UseCases.AuthUseCases.RefreshAccessTokenUseCase;

public sealed record RefreshAccessTokenRequest(string refreshToken) : IRequest<string> { }