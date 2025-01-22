using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SupportService.Domain.Abstractions;
using SupportService.Domain.Entities;
using SupportService.Infrastructure.Data;
using SupportService.Infrastructure.Repositories;
using SupportService.Tests.Factories;
using Testcontainers.MySql;

namespace SupportService.Tests.IntegrationTests.Repositories;

public class RepositoryTests : IAsyncLifetime
{
    private readonly MySqlContainer _dbContainer;
    private readonly IServiceProvider _serviceProvider;
    private readonly Fixture _fixture = new();
    
    public RepositoryTests()
    {
        _dbContainer = new MySqlBuilder()
            .WithImage("mysql:8.0.36")
            .WithDatabase("shopsupportdb")
            .WithUsername("root")
            .WithPassword("root")
            .Build();
        
        _serviceProvider = IntegrationTestsEntitiesFactory.CreateTestServiceProvider(_dbContainer);
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        
        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();
        await unitOfWork.EnableMigrationsAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    private AppDbContext GetDbContext()
    {
        return _serviceProvider.GetRequiredService<AppDbContext>();
    }

    private Chat GetChatEntity(string clientId)
    {
        return _fixture.Build<Chat>()
            .Without(chat => chat.Messages)
            .With(chat => chat.ClientId, clientId)
            .Create();
    }

    [Fact]
    public async Task AddAsync_WhenCalled_ShouldAddEntityToDatabase()
    {
        //Arrange
        var dbContext = GetDbContext();
        var repository = new Repository<Chat>(dbContext);

        var entity = GetChatEntity(_fixture.Create<string>());

        //Act
        await repository.AddAsync(entity);
        
        await dbContext.SaveChangesAsync();

        var result = await dbContext.Set<Chat>().FirstOrDefaultAsync(e => e.Id == entity.Id);

        //Assert
        Assert.NotNull(result);
        Assert.Equivalent(entity, result);
    }

    [Fact]
    public async Task ListAllAsync_WhenCalled_ShouldReturnAllEntities()
    {
        //Arrange
        var dbContext = GetDbContext();
        var repository = new Repository<Chat>(dbContext);
    
        var entities = new List<Chat>
        {
            GetChatEntity(_fixture.Create<string>()),
            GetChatEntity(_fixture.Create<string>()),
        };
    
        await dbContext.Set<Chat>().AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    
        //Act
        var result = await repository.ListAllAsync();
    
        //Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ListAsync_WhenCalled_ShouldFilterEntities()
    {
        //Arrange
        var dbContext = GetDbContext();
        var repository = new Repository<Chat>(dbContext);
    
        var entities = new List<Chat>
        {
            GetChatEntity(_fixture.Create<string>()),
            GetChatEntity(_fixture.Create<string>()),
        };
    
        await dbContext.Set<Chat>().AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    
        //Act
        var result = await repository.ListAsync(chat => chat.ClientId == entities[0].ClientId);
    
        //Assert
        Assert.Single(result);
        Assert.Equivalent(entities[0], result.First());
    }
    
    [Fact]
    public async Task FirstOrDefaultAsync_WhenCalled_ShouldReturnFirstMatchedEntity()
    {
        //Arrange
        var dbContext = GetDbContext();
        var repository = new Repository<Chat>(dbContext);
    
        var entities = new List<Chat>
        {
            GetChatEntity(_fixture.Create<string>()),
            GetChatEntity("id"),
        };
    
        await dbContext.Set<Chat>().AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    
        //Act
        var result = await repository.FirstOrDefaultAsync(x => x.ClientId == "Id");
    
        //Assert
        Assert.NotNull(result);
        Assert.Equivalent(entities[1], result);
    }
    
    [Fact]
    public async Task DeleteAsync_WhenCalled_ShouldRemoveEntityFromDatabase()
    {
        //Arrange
        var dbContext = GetDbContext();
        var repository = new Repository<Chat>(dbContext);
    
        var entity = GetChatEntity(_fixture.Create<string>());
    
        await dbContext.Set<Chat>().AddAsync(entity);
        await dbContext.SaveChangesAsync();
    
        //Act
        await repository.DeleteAsync(entity);
        await dbContext.SaveChangesAsync();
    
        var result = await repository.GetByIdAsync(entity.Id);
        var resultList = await repository.ListAllAsync();
        
        //Assert
        Assert.Null(result);
        Assert.Empty(resultList);
    }
}