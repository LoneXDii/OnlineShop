using AutoFixture;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Models;
using OrderService.Tests.Factories;
using Testcontainers.MongoDb;

namespace OrderService.Tests.IntegrationTests.Repositories;

public class OrderRepositoryTests : IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer;
    private readonly Fixture _fixture = new();
    private IServiceProvider _serviceProvider;

    public OrderRepositoryTests()
    {
        _mongoDbContainer = new MongoDbBuilder()
            .WithImage("mongo")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();
        
        _serviceProvider = IntegrationTestsEntitiesFactory.CreateServiceProviderForMongoDbTests(_mongoDbContainer);
    }

    public async Task DisposeAsync()
    {
        await _mongoDbContainer.StopAsync();
    }

    private OrderEntity CreateOrderEntity()
    {
        return _fixture.Build<OrderEntity>()
            .With(order => order.Id, ObjectId.GenerateNewId().ToString())
            .With(order => order.Products, [])
            .Create();
    }
    
    [Fact]
    public async Task CreateAsync_WhenCalled_ShouldInsertOrderIntoDatabase()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

        var order = CreateOrderEntity();
        
        //Act
        await repository.CreateAsync(order);

        //Assert
        var mongoCollection = scope.ServiceProvider.GetRequiredService<IMongoCollection<Order>>();
        
        var count = await mongoCollection.CountDocumentsAsync(Builders<Order>.Filter.Empty, null);
        
        var savedOrder = await mongoCollection.Find(Builders<Order>.Filter.Empty).SingleOrDefaultAsync();
        
        order.CreatedAt = savedOrder.CreatedAt;
        
        Assert.Equal(1, count);
        Assert.NotNull(savedOrder);
        Assert.Equivalent(order, savedOrder);
    }
    
    [Fact]
    public async Task ListWithPaginationAsync_WhenCalledWithoutFilters_ShouldReturnPaginatedOrders()
    {
        //Arrange
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

        var mongoCollection = scope.ServiceProvider.GetRequiredService<IMongoCollection<Order>>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        
        for (var i = 0; i < 15; i++)
        {
            var order = CreateOrderEntity();
            
            await mongoCollection.InsertOneAsync(mapper.Map<Order>(order), null);
        }

        //Act
        var page1 = await repository.ListWithPaginationAsync(1, 10);
        
        var page2 = await repository.ListWithPaginationAsync(2, 10);

        //Assert
        Assert.Equal(10, page1.Count);
        Assert.Equal(5, page2.Count); 
    }
    
    [Fact]
    public async Task ListWithPaginationAsync_WhenFiltersProvided_ShouldReturnPaginatedOrders()
    {
        //Arrange
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

        var mongoCollection = scope.ServiceProvider.GetRequiredService<IMongoCollection<Order>>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        
        for (var i = 0; i < 15; i++)
        {
            var order = CreateOrderEntity();
            order.TotalPrice = i + 1;
            
            await mongoCollection.InsertOneAsync(mapper.Map<Order>(order), null);
        }

        //Act
        var page1 = await repository.ListWithPaginationAsync(1, 10, CancellationToken.None,
            order => order.TotalPrice <= 12);
        
        var page2 = await repository.ListWithPaginationAsync(2, 10, CancellationToken.None,
            order => order.TotalPrice <= 12);

        //Assert
        Assert.Equal(10, page1.Count);
        Assert.Equal(2, page2.Count); 
    }
    
    [Fact]
    public async Task GetByIdAsync_WhenCalled_ShouldReturnCorrectOrder()
    {
        //Arrange
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

        var order = CreateOrderEntity();
        var mongoCollection = scope.ServiceProvider.GetRequiredService<IMongoCollection<Order>>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        
        await mongoCollection.InsertOneAsync(mapper.Map<Order>(order), null);

        //Act
        var fetchedOrder = await repository.GetByIdAsync(order.Id);

        //Assert
        order.CreatedAt = fetchedOrder.CreatedAt;
        
        Assert.NotNull(fetchedOrder);
        Assert.Equivalent(order, fetchedOrder);
    }
    
    [Fact]
    public async Task UpdateAsync_WhenCalled_ShouldUpdateOrderInDatabase()
    {
        //Arrange
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

        var order = CreateOrderEntity();
        var mongoCollection = scope.ServiceProvider.GetRequiredService<IMongoCollection<Order>>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        
        await mongoCollection.InsertOneAsync(mapper.Map<Order>(order), null);

        //Act
        var newPrice = 1234;
        order.TotalPrice = newPrice;
        
        await repository.UpdateAsync(order);

        //Assert
        var updatedOrder = await mongoCollection.Find(o => o.Id == order.Id)
            .FirstOrDefaultAsync();
        
        Assert.NotNull(updatedOrder);
        Assert.Equal(newPrice, updatedOrder.TotalPrice);
    }
    
    [Fact]
    public async Task CountAsync_WhenCalledWithoutFilters_ShouldReturnCorrectCount()
    {
        //Arrange
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

        var mongoCollection = scope.ServiceProvider.GetRequiredService<IMongoCollection<Order>>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        
        for (var i = 0; i < 5; i++)
        {
            var order = CreateOrderEntity();
            
            await mongoCollection.InsertOneAsync(mapper.Map<Order>(order), null);
        }

        //Act
        var count = await repository.CountAsync();

        //Assert
        Assert.Equal(5, count);
    }
    
    [Fact]
    public async Task CountAsync_WithFiltersProvided_ShouldReturnFilteredCount()
    {
        //Arrange
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

        var mongoCollection = scope.ServiceProvider.GetRequiredService<IMongoCollection<Order>>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        
        for (var i = 0; i < 5; i++)
        {
            var order = CreateOrderEntity();
            order.TotalPrice = i + 1;
            
            await mongoCollection.InsertOneAsync(mapper.Map<Order>(order), null);
        }

        //Act
        var count = await repository.CountAsync(filters: order => order.TotalPrice < 3);

        //Assert
        Assert.Equal(2, count);
    }
}