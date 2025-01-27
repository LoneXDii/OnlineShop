using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Infrastructure.Data;
using ProductsService.Tests.Factories;
using Testcontainers.MySql;

namespace ProductsService.Tests.IntegrationTests.Repositories;

public class UnitOfWorkTests : IAsyncLifetime
{
    private readonly MySqlContainer _commandDbContainer;
    private readonly MySqlContainer _queryDbContainer;
    private readonly IServiceProvider _serviceProvider;

    public UnitOfWorkTests()
    {
        _commandDbContainer = new MySqlBuilder()
            .WithImage("mysql:8.0.36")
            .WithDatabase("products")
            .WithUsername("root")
            .WithPassword("root")
            .Build();

        _queryDbContainer = new MySqlBuilder()
            .WithImage("mysql:8.0.36")
            .WithDatabase("products")
            .WithUsername("root")
            .WithPassword("root")
            .Build();
        
        _serviceProvider = IntegrationTestsEntitiesFactory.CreateTestServiceProvider(_commandDbContainer, _queryDbContainer);
    }
    
    [Fact]
    public async Task EnableMigrationsAsync_WhenCalled_ShouldCreateDatabase()
    {
        //Arrange
        using var scope = _serviceProvider.CreateScope(); ;
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var dbContext = scope.ServiceProvider.GetRequiredService<CommandDbContext>();
         
        //Act
        var isPendingMigrationsExistsBeforeMigration = dbContext.Database.GetPendingMigrations().Any();
         
        await unitOfWork.EnableMigrationsAsync();
        
        //Assert
        Assert.True(isPendingMigrationsExistsBeforeMigration);
        Assert.False(dbContext.Database.GetPendingMigrations().Any());
        Assert.Equal(21, dbContext.Categories.Count());
        Assert.Equal(4, dbContext.Products.Count());
        Assert.Equal(1, dbContext.Discounts.Count());
    }

    public Task InitializeAsync()
    {
        var commandContainerTask = _commandDbContainer.StartAsync();
        var queryContainerTask = _queryDbContainer.StartAsync();
        
        Task.WaitAll(commandContainerTask, queryContainerTask);
        
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        var commandContainerTask = _commandDbContainer.StopAsync();
        var queryContainerTask = _queryDbContainer.StopAsync();
        
        Task.WaitAll(commandContainerTask, queryContainerTask);
        
        return Task.CompletedTask;
    }
}