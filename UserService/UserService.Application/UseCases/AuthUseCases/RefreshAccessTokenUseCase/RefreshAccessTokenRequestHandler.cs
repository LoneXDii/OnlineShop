using MediatR;
using UserService.Infrastructure.Services.Authentication;

namespace UserService.Application.UseCases.AuthUseCases.RefreshAccessTokenUseCase;

internal class RefreshAccessTokenRequestHandler(ITokenService tokenService)
	: IRequestHandler<RefreshAccessTokenRequest, string>
{
	public async Task<string> Handle(RefreshAccessTokenRequest request, CancellationToken cancellationToken)
	{
		var token = await tokenService.RefreshAccessTokenAsync(request.refreshToken);
		return token;
	}
}
