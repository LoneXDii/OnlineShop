using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SupportService.Domain.Abstractions;
using SupportService.Infrastructure.Data;
using SupportService.Tests.Factories;
using Testcontainers.MySql;

namespace SupportService.Tests.IntegrationTests.Repositories;

public class UnitOfWorkTests : IAsyncDisposable
{
    private readonly MySqlContainer _dbContainer;
    private readonly IServiceProvider _serviceProvider;

    public UnitOfWorkTests()
    {
        _dbContainer = new MySqlBuilder()
            .WithImage("mysql:8.0.36")
            .WithDatabase("shopsupportdb")
            .WithUsername("root")
            .WithPassword("root")
            .Build();

        _serviceProvider = IntegrationTestsEntitiesFactory.CreateTestServiceProvider(_dbContainer);
    }

    [Fact]
         public async Task EnableMigrationsAsync_WhenCalled_ShouldCreateDatabase()
         {
             //Arrange
             await _dbContainer.StartAsync();
             using var scope = _serviceProvider.CreateScope(); ;
             var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
             var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
             
             //Act
             var isPendingMigrationsExistsBeforeMigration = dbContext.Database.GetPendingMigrations().Any();
             
             await unitOfWork.EnableMigrationsAsync();
             
             //Assert
             Assert.True(isPendingMigrationsExistsBeforeMigration);
             Assert.False(dbContext.Database.GetPendingMigrations().Any());
             await _dbContainer.StopAsync();
         }
    
    public async ValueTask DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}