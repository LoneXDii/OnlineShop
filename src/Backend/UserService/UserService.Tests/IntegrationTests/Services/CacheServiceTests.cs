using AutoFixture;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.Redis;
using UserService.DAL.Services.TemporaryStorage;
using UserService.Tests.Factories;

namespace UserService.Tests.IntegrationTests.Services;

public class CacheServiceTests : IAsyncLifetime
{
    private readonly RedisContainer _redisContainer;
    private readonly Fixture _fixture = new();
    private IServiceProvider _serviceProvider;
    private ICacheService _redisStorageService;
    
    public CacheServiceTests()
    {
        _redisContainer = new RedisBuilder()
            .WithImage("redis:latest")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _redisContainer.StartAsync();

        _serviceProvider = IntegrationTestsEntitiesFactory.CreateServiceProviderForRedisTests(_redisContainer);
        using var scope = _serviceProvider.CreateScope();
        
        _redisStorageService = scope.ServiceProvider.GetRequiredService<ICacheService>();
    }

    public async Task DisposeAsync()
    {
        await _redisContainer.StopAsync();
    }

    [Fact]
    public async Task GetEmailByCodeAsync_WhenCalled_ShouldReturnEmailFromRedis()
    {
        //Arrange
        var email = "email@email.com";
        
        using var scope = _serviceProvider.CreateScope();
        var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
        
        await cache.SetStringAsync("emailCode", email);
        
        //Act
        var result = await _redisStorageService.GetEmailByCodeAsync("Code");
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(email, result);
    }

    [Fact]
    public async Task SetEmailConfirmationCodeAsync_WhenCalled_ShouldSaveEmailInRedisAndReturnCode()
    {
        //Arrange
        var email = "email@email.com";
        
        //Act
        var code = await _redisStorageService.SetEmailConfirmationCodeAsync(email);
        
        //Assert
        using var scope = _serviceProvider.CreateScope();
        var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
        
        var emailInRedis = await cache.GetStringAsync($"email{code}");
        
        Assert.NotNull(emailInRedis);
        Assert.Equal(email, emailInRedis);
    }
    
    [Fact]
    public async Task GetEmailByResetPasswordCodeAsync_WhenCalled_ShouldReturnEmailFromRedis()
    {
        //Arrange
        var email = "email@email.com";
        
        using var scope = _serviceProvider.CreateScope();
        var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
        
        await cache.SetStringAsync("passwordCode", email);
        
        //Act
        var result = await _redisStorageService.GetEmailByResetPasswordCodeAsync("Code");
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(email, result);
    }

    [Fact]
    public async Task SetResetPasswordCodeAsync_WhenCalled_ShouldSaveEmailInRedisAndReturnCode()
    {
        //Arrange
        var email = "email@email.com";
        
        //Act
        var code = await _redisStorageService.SetResetPasswordCodeAsync(email);
        
        //Assert
        using var scope = _serviceProvider.CreateScope();
        var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
        
        var emailInRedis = await cache.GetStringAsync($"password{code}");
        
        Assert.NotNull(emailInRedis);
        Assert.Equal(email, emailInRedis);
    }
}