using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Common.Models;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Configuration;
using System.Linq.Expressions;

namespace OrderService.Infrastructure.Services;

internal class MongoOrderRepository : IOrderRepository
{
    private readonly IMongoCollection<OrderEntity> _ordersCollection;

    public MongoOrderRepository(IOptions<MongoDBSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionURI);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _ordersCollection = database.GetCollection<OrderEntity>(settings.Value.CollectionName);
    }

    public async Task CreateAsync(OrderEntity order)
    {
        await _ordersCollection.InsertOneAsync(order);
    }

    public async Task<PaginatedListModel<OrderEntity>> ListWithPaginationAsync(int pageNo = 1, int pageSize = 10, 
        params Expression<Func<OrderEntity, bool>>[] filters)
    {
        var mongoFilters = new List<FilterDefinition<OrderEntity>>();

        foreach (var filter in filters)
        {
            var mongoFilter = Builders<OrderEntity>.Filter.Where(filter);
            mongoFilters.Add(mongoFilter);
        }

        var combinedFilter = mongoFilters.Any()
            ? Builders<OrderEntity>.Filter.And(mongoFilters)
            : Builders<OrderEntity>.Filter.Empty;

        var orders = await _ordersCollection.Find(combinedFilter)
            .SortByDescending(order => order.CreatedAt)
            .Skip((pageNo - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        var count = await _ordersCollection.CountDocumentsAsync(combinedFilter);

        var data = new PaginatedListModel<OrderEntity>
        {
            Items = orders,
            CurrentPage = pageNo,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize)
        };

        return data;
    }

    public async Task<OrderEntity?> GetByIdAsync(string id)
    {
        var order = await _ordersCollection.Find(order => order.Id == id).FirstOrDefaultAsync();

        return order;
    }

    public async Task UpdateAsync(OrderEntity order)
    {
        var filter = Builders<OrderEntity>.Filter.Where(o => o.Id == order.Id);

        await _ordersCollection.ReplaceOneAsync(filter, order);
    }
}
