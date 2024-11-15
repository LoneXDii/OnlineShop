using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Infrastructure.Entities;
using UserService.Infrastructure.Models;

namespace UserService.Infrastructure.Services.Authentication;

internal class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<AppUser> _userManager;

    public TokenService(IConfiguration configuration, UserManager<AppUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<TokensDTO> GetTokensAsync(AppUser user)
    {
        var tokens = new TokensDTO();

        tokens.AccessToken = await GetAccessTokenAsync(user);
        tokens.RefreshToken = await GetRefreshTokenAsync(user);

        return tokens;
    }

    public async Task<string?> RefreshAccessTokenAsync(string refreshToken)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.RefreshTokenValue == refreshToken);
        if(user is null || user?.RefreshTokenExpiresAt < DateTime.Now)
        {
            return null;
        }

        var token = await GetAccessTokenAsync(user);
        return token;
    }

    public async Task InvalidateRefreshTokenAsync(AppUser user)
    {
        user.RefreshTokenValue = null;
		await _userManager.UpdateAsync(user);
	}

	public async Task<string> GetRefreshTokenAsync(AppUser user)
	{
		user.RefreshTokenValue = Guid.NewGuid().ToString();
		user.RefreshTokenExpiresAt = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JWT:RefreshExpireDays"]));
		await _userManager.UpdateAsync(user);

		return user.RefreshTokenValue;
	}

	private async Task<string> GetAccessTokenAsync(AppUser user)
    {
        var claims = new List<Claim>
        {
             new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             new(ClaimTypes.Email, user.Email),
             new("Avatar", user.AvatarUrl ?? ""),
             new("Id" , user.Id)
        };

		var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
		var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JWT:AccessExpireMinutes"]));

        var token = new JwtSecurityToken
            (
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: expires,
                claims: claims,
                signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
