using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Configuration;
using System.Linq.Expressions;

namespace OrderService.Infrastructure.Repositories;

internal class MongoOrderRepository : IOrderRepository
{
    private readonly IMongoCollection<OrderEntity> _ordersCollection;

    public MongoOrderRepository(IOptions<MongoDBSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionURI);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _ordersCollection = database.GetCollection<OrderEntity>(settings.Value.CollectionName);
    }

    public async Task CreateAsync(OrderEntity order, CancellationToken cancellationToken = default)
    {
        await _ordersCollection.InsertOneAsync(order, null, cancellationToken);
    }

    public async Task<List<OrderEntity>> ListWithPaginationAsync(int pageNo = 1, int pageSize = 10,
        CancellationToken cancellationToken = default,
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

        return await _ordersCollection.Find(combinedFilter)
            .SortByDescending(order => order.CreatedAt)
            .Skip((pageNo - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<OrderEntity?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _ordersCollection.Find(order => order.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAsync(OrderEntity order, CancellationToken cancellationToken = default)
    {
        var filter = Builders<OrderEntity>.Filter.Where(o => o.Id == order.Id);
        var replaceOptions = new ReplaceOptions();

        await _ordersCollection.ReplaceOneAsync(filter, order, replaceOptions, cancellationToken);
    }

    public async Task<long> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _ordersCollection.CountDocumentsAsync(Builders<OrderEntity>.Filter.Empty, null, cancellationToken);
    }

    public async Task<long> CountAsync(CancellationToken cancellationToken = default,
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

        return await _ordersCollection.CountDocumentsAsync(combinedFilter, null, cancellationToken);
    }
}
