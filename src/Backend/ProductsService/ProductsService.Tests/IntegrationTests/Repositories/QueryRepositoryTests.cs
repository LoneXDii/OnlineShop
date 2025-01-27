using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductsService.Application.Specifications;
using ProductsService.Application.Specifications.Products;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;
using ProductsService.Infrastructure.Data;
using ProductsService.Infrastructure.Repositories;
using ProductsService.Tests.Factories;
using Testcontainers.MySql;

namespace ProductsService.Tests.IntegrationTests.Repositories;

public class QueryRepositoryTests : IAsyncLifetime
{
    private readonly MySqlContainer _dbContainer;
    private readonly IServiceProvider _serviceProvider;
    private readonly Fixture _fixture = new();

    public QueryRepositoryTests()
    {
        _dbContainer = new MySqlBuilder()
            .WithImage("mysql:8.0.36")
            .WithDatabase("querysupportdb")
            .WithUsername("root")
            .WithPassword("root")
            .Build();

        _serviceProvider = IntegrationTestsEntitiesFactory.CreateTestServiceProvider(_dbContainer, _dbContainer);
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();
        
        await unitOfWork.EnableMigrationsAsync();
        
        var dbContext = GetDbContext();
        
        var entities = await dbContext.Set<Product>().ToListAsync();
        
        dbContext.Set<Product>().RemoveRange(entities);
        
        await dbContext.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    private QueryDbContext GetDbContext()
    {
        return _serviceProvider.GetRequiredService<QueryDbContext>();
    }

    private Product CreateEntity(double price = 1)
    {
        return _fixture.Build<Product>()
            .Without(p => p.Discount)
            .Without(p => p.Categories)
            .With(p => p.Price, price)
            .Create();
    }

    [Fact]
    public async Task GetByIdAsync_WhenCalled_ShouldReturnEntityById()
    {
        //Arrange
        var dbContext = GetDbContext();
        var repository = new QueryRepository<Product>(dbContext);
        var entity = CreateEntity();
        
        await dbContext.Set<Product>().AddAsync(entity);
        
        await dbContext.SaveChangesAsync();

        //Act
        var result = await repository.GetByIdAsync(entity.Id);

        //Assert
        Assert.NotNull(result);
        Assert.Equivalent(entity, result);
    }

    [Fact]
    public async Task ListAllAsync_WhenCalled_ShouldReturnAllEntities()
    {
        //Arrange
        var dbContext = GetDbContext();
        var repository = new QueryRepository<Product>(dbContext);
    
        var entities = new List<Product>
        {
            CreateEntity(),
            CreateEntity()
        };
    
        await dbContext.Set<Product>().AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    
        //Act
        var result = await repository.ListAllAsync();
    
        //Assert
        Assert.Equal(2, result.Count());
    }
    
    [Fact]
    public async Task ListAsync_WhenCalled_ShouldReturnFilteredEntities()
    {
        //Arrange
        var dbContext = GetDbContext();
        var repository = new QueryRepository<Product>(dbContext);
    
        var entities = new List<Product>
        {
            CreateEntity(1),
            CreateEntity(2)
        };
        await dbContext.Set<Product>().AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();

        var spec = new ProductMinPriceSpecification(2);
    
        //Act
        var result = await repository.ListAsync(spec);
    
        //Assert
        Assert.Single(result);
        Assert.Equivalent(entities[1], result.First());
    }
    
    [Fact]
    public async Task ListWithPaginationAsync_WhenCalled_ShouldReturnPaginatedEntities()
    {
        //Arrange
        var dbContext = GetDbContext();
        var repository = new QueryRepository<Product>(dbContext);
    
        var entities = new List<Product>
        {
            CreateEntity(1),
            CreateEntity(2),
            CreateEntity(3),
            CreateEntity(4)
        };
    
        await dbContext.Set<Product>().AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    
        var spec = new ProductMinPriceSpecification(2);
    
        //Act
        var result = await repository.ListWithPaginationAsync(1, 2, spec);
    
        //Assert
        Assert.Equal(2, result.Count);
    }
    
    [Fact]
    public async Task FirstOrDefaultAsync_WhenCalled_ShouldReturnFirstMatchingEntity()
    {
        //Arrange
        var dbContext = GetDbContext();
        var repository = new QueryRepository<Product>(dbContext);
    
        var entities = new List<Product>
        {
            CreateEntity(1),
            CreateEntity(2),
        };
    
        await dbContext.Set<Product>().AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    
        var spec = new ProductMinPriceSpecification(2);
    
        //Act
        var result = await repository.FirstOrDefaultAsync(spec);
    
        //Assert
        Assert.NotNull(result);
        Assert.Equivalent(entities[1], result);
    }
    
    [Fact]
    public async Task CountAsync_WhenCalled_ShouldReturnCorrectCount()
    {
        //Arrange
        var dbContext = GetDbContext();
        var repository = new QueryRepository<Product>(dbContext);
    
        var entities = new List<Product>
        {
            CreateEntity(1),
            CreateEntity(2),
            CreateEntity(3),
        };
    
        await dbContext.Set<Product>().AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    
        var spec = new ProductMinPriceSpecification(2);
    
        //Act
        var result = await repository.CountAsync(spec);
    
        //Assert
        Assert.Equal(2, result);
    }
}