using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Testcontainers.Redis;
using UserService.DAL.Services.TemporaryStorage;

namespace UserService.Tests.Factories;

public static class IntegrationTestsEntitiesFactory
{
    public static IServiceProvider CreateServiceProviderForRedisTests(RedisContainer redisContainer)
    {
        var services = new ServiceCollection();
        
        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = redisContainer.GetConnectionString();
            opt.InstanceName = "codes";
        });
        
        var loggerMock = new Mock<ILogger<CacheService>>();
        services.AddSingleton(loggerMock.Object);
        
        services.AddScoped<ICacheService, CacheService>();
        
        return services.BuildServiceProvider();
    }
}