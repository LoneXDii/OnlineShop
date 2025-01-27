using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text.Json;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;
using OrderService.Tests.Factories;
using Testcontainers.Redis;

namespace OrderService.Tests.IntegrationTests.Services;

public class RedisStorageServiceTests : IAsyncLifetime
{
    private readonly RedisContainer _redisContainer;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();
    private readonly HttpContext _httpContext;
    private readonly Fixture _fixture = new();
    private IServiceProvider _serviceProvider;
    private ITemporaryStorageService _redisStorageService;
    
    public RedisStorageServiceTests()
    {
        _redisContainer = new RedisBuilder()
            .WithImage("redis:latest")
            .Build();
        
        _httpContext = new DefaultHttpContext();
        
        _httpContextAccessorMock.Setup(accessor => accessor.HttpContext).Returns(_httpContext);
    }

    public async Task InitializeAsync()
    {
        await _redisContainer.StartAsync();

        _serviceProvider = IntegrationTestsEntitiesFactory.CreateServiceProviderForRedisTests(_redisContainer, _httpContextAccessorMock.Object);
        using var scope = _serviceProvider.CreateScope();
        
        _redisStorageService = scope.ServiceProvider.GetRequiredService<ITemporaryStorageService>();
    }

    public async Task DisposeAsync()
    {
        await _redisContainer.StopAsync();
    }

    [Fact]
    public async Task SaveCartAsync_WhenUserIdIsInHttpContext_ShouldSaveCartInRedisWithUserIdAsKey()
    {
        //Arrange
        var cart = _fixture.Create<Dictionary<int, ProductEntity>>();
        _httpContext.User = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim("Id", "Id")
        ]));
        
        //Act
        await _redisStorageService.SaveCartAsync(cart);
        
        //Assert
        var cartJson = JsonSerializer.Serialize(cart);
        
        using var scope = _serviceProvider.CreateScope();
        var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
        
        var cartInRedis = await cache.GetStringAsync("Id");
        
        Assert.Equal(cartJson, cartInRedis);
    }
    
    [Fact]
    public async Task SaveCartAsync_WhenUserIdIsInNullAndCartIdIsInCookies_ShouldSaveCartInRedisWithIdFromCookiesAsKey()
    {
        //Arrange
        var cart = _fixture.Create<Dictionary<int, ProductEntity>>();
        _httpContext.Request.Cookies = new TestRequestCookieCollection(new Dictionary<string, string>
        {
            { "CartId", "cartId" } 
        });
        
        //Act
        await _redisStorageService.SaveCartAsync(cart);
        
        //Assert
        var cartJson = JsonSerializer.Serialize(cart);
        
        using var scope = _serviceProvider.CreateScope();
        var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
        
        var cartInRedis = await cache.GetStringAsync("cartId");
        
        Assert.Equal(cartJson, cartInRedis);
    }
    
    [Fact]
    public async Task SaveCartAsync_WhenUserIdIsInNullAndNoCartIdInCookies_ShouldSaveCartInRedisWithNewIdAndPassItToResponseCookies()
    {
        //Arrange
        var cart = _fixture.Create<Dictionary<int, ProductEntity>>();
        
        //Act
        await _redisStorageService.SaveCartAsync(cart);
        
        //Assert
        var cartJson = JsonSerializer.Serialize(cart);
        
        using var scope = _serviceProvider.CreateScope();
        var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
        
        _httpContext.Response.Headers.TryGetValue("Set-Cookie", out var setCookie);
        var cookie = setCookie.Single();
        var tokens = cookie.Split(';');
        var firstToken = tokens.First();
        var keyValuePair = firstToken.Split('=');
        var cartId = keyValuePair[1];
        
        var cartInRedis = await cache.GetStringAsync(cartId);
        
        Assert.Equal(cartJson, cartInRedis);
    }

    [Fact]
    public async Task GetCartAsync_WhenNoUserIdInContext_ShouldReturnCartAssignedToIdInCookies()
    {
        //Arrange
        var cart = _fixture.Create<Dictionary<int, ProductEntity>>();
        _httpContext.Request.Cookies = new TestRequestCookieCollection(new Dictionary<string, string>
        {
            { "CartId", "cartId" } 
        });
        
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
        };

        using var scope = _serviceProvider.CreateScope();
        var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
        var cartJson = JsonSerializer.Serialize(cart);
        
        await cache.SetStringAsync("cartId", cartJson, options);
        
        //Act
        var cartResponse = await _redisStorageService.GetCartAsync();
        
        //Assert
        var responseJson = JsonSerializer.Serialize(cartResponse);
        
        Assert.Equal(cartJson, responseJson);
    }
    
    [Fact]
    public async Task GetCartAsync_WhenUserIdIsInContextAndNoCartIdInCookies_ShouldReturnEmptyCartAssignedToUserIdAndRemoveCartIdFromCookies()
    {
        //Arrange
        _httpContext.User = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim("Id", "Id")
        ]));
        
        //Act
        var cartResponse = await _redisStorageService.GetCartAsync();
        
        //Assert
        Assert.NotNull(cartResponse);
        Assert.Empty(cartResponse);
    }
    
    [Fact]
    public async Task GetCartAsync_WhenUserIdIsInContextButNoCartAssignedToIdFromCookies_ShouldReturnEmptyCartAssignedToUserId()
    {
        //Arrange
        var cart = _fixture.Create<Dictionary<int, ProductEntity>>();
        _httpContext.Request.Cookies = new TestRequestCookieCollection(new Dictionary<string, string>
        {
            { "CartId", "cartId" } 
        });
        
        _httpContext.User = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim("Id", "Id")
        ]));
        
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
        };

        using var scope = _serviceProvider.CreateScope();
        var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
        var cartJson = JsonSerializer.Serialize(cart);
        
        await cache.SetStringAsync("cartId", cartJson, options);
        
        //Act
        var cartResponse = await _redisStorageService.GetCartAsync();
        
        //Assert
        var oldCartJson = await cache.GetStringAsync("cartId");
        var newCartJson = await cache.GetStringAsync("Id");
        
        var responseJson = JsonSerializer.Serialize(cartResponse);
        
        Assert.Equal(cartJson, responseJson);
        Assert.Equal(cartJson, newCartJson);
        Assert.Null(oldCartJson);
        Assert.NotNull(newCartJson);
    }
    
    [Fact]
    public async Task GetCartAsync_WhenUserIdIsInContextAndExistsCartAssignedToIdFromCookies_ShouldReassignCartToUserId()
    {
        //Arrange
        _httpContext.Request.Cookies = new TestRequestCookieCollection(new Dictionary<string, string>
        {
            { "CartId", "cartId" } 
        });
        
        _httpContext.User = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim("Id", "Id")
        ]));
        
        //Act
        var cartResponse = await _redisStorageService.GetCartAsync();
        
        //Assert
        Assert.NotNull(cartResponse);
        Assert.Empty(cartResponse);
    }
}

public class TestRequestCookieCollection : IRequestCookieCollection
{
    private readonly Dictionary<string, string> _cookies;

    public TestRequestCookieCollection(Dictionary<string, string> cookies)
    {
        _cookies = cookies;
    }

    public string this[string key] => _cookies.TryGetValue(key, out var value) ? value : null;

    public ICollection<string> Keys => _cookies.Keys;

    public bool TryGetValue(string key, [NotNullWhen(true)] out string? value)
    {
       return _cookies.TryGetValue(key, out value);
    }

    public int Count => _cookies.Count;

    public bool ContainsKey(string key) => _cookies.ContainsKey(key);

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _cookies.GetEnumerator();

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}