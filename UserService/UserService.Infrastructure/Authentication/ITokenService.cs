using Microsoft.AspNetCore.Identity;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Authentication;

internal interface ITokenService
{
	Task<string> GetAccessToken(AppUser user);

	Task<string> GetRefreshToken(AppUser user);

	Task<string> RefreshAccessToken(string refreshToken);
}
