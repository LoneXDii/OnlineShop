using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.DAL.Entities;
using UserService.DAL.Services.Authentication;
using UserService.Tests.Factories;

namespace UserService.Tests.UnitTests.Services;

public class TokenServiceTests
{
    private readonly Mock<IConfiguration> _configurationMock = new();
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly TokenService _tokenService;
    private readonly Fixture _fixture = new();

    public TokenServiceTests()
    {
        _userManagerMock = MocksFactory.CreateUserManager();
        var loggerMock = new Mock<ILogger<TokenService>>();
        
        _configurationMock.Setup(configuration => configuration["JWT:Secret"])
            .Returns("TempSecretKey123123123123123123123");
        
        _configurationMock.Setup(configuration => configuration["JWT:AccessExpireMinutes"])
            .Returns("30");
        
        _configurationMock.Setup(configuration => configuration["JWT:RefreshExpireDays"])
            .Returns("7");
        
        _userManagerMock.Setup(manager => manager.GetRolesAsync(It.IsAny<AppUser>()))
            .ReturnsAsync(new List<string> { "Admin" });
        
        _tokenService = new TokenService(_configurationMock.Object, _userManagerMock.Object, loggerMock.Object);
    }

    [Fact]
    public async Task GetTokensAsync_WhenCalled_ShouldReturnRefreshAndAccessTokensValues()
    {
        //Arrange
        var user = _fixture.Build<AppUser>()
            .Without(u => u.RefreshTokenValue)
            .Without(u => u.RefreshTokenExpiresAt)
            .Create();
        
        //Act
        var tokens = await _tokenService.GetTokensAsync(user);
        
        //Assert
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(tokens.AccessToken);
        var expClaim = jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Exp).Value;
        var expDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim)).UtcDateTime;
        
        Assert.NotNull(user.RefreshTokenValue);
        Assert.NotNull(user.RefreshTokenExpiresAt);
        Assert.Equal(user.RefreshTokenValue, tokens.RefreshToken);
        Assert.True(user.RefreshTokenExpiresAt > DateTime.UtcNow.AddDays(6));
        
        Assert.True(expDateTime > DateTime.UtcNow.AddMinutes(29));
        Assert.True(expDateTime < DateTime.UtcNow.AddMinutes(31));
        Assert.Equal(user.Email, jwtToken.Claims.First(c => c.Type == ClaimTypes.Email).Value);
        Assert.Equal(user.Id, jwtToken.Claims.First(c => c.Type == "Id").Value);
        Assert.Contains("Admin", 
            jwtToken.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value));
        
        _userManagerMock.Verify(manager => manager.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task InvalidateRefreshTokenAsync_WhenCalled_ShouldSetRefreshTokenFieldToNullAndSaveInDb()
    {
        //Arrange
        var user = _fixture.Create<AppUser>();
        
        //Act
        await _tokenService.InvalidateRefreshTokenAsync(user);
        
        //Assert
        Assert.Null(user.RefreshTokenValue);
        _userManagerMock.Verify(manager => manager.UpdateAsync(user), Times.Once);
    }
    
    [Fact]
    public async Task GetRefreshTokenAsync_WhenCalled_ShouldSetRefreshTokenToUserAndSaveInDb()
    {
        //Arrange
        var user = _fixture.Build<AppUser>()
            .Without(u => u.RefreshTokenValue)
            .Without(u => u.RefreshTokenExpiresAt)
            .Create();
        
        //Act
        var token = await _tokenService.GetRefreshTokenAsync(user);
        
        //Assert
        Assert.NotNull(user.RefreshTokenValue);
        Assert.NotNull(user.RefreshTokenExpiresAt);
        Assert.True(user.RefreshTokenExpiresAt > DateTime.UtcNow.AddDays(6));
        _userManagerMock.Verify(manager => manager.UpdateAsync(user), Times.Once);
    }
}
