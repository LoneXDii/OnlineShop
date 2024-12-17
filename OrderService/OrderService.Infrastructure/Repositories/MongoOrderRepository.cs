using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Configuration;
using OrderService.Infrastructure.Models;
using System.Linq.Expressions;

namespace OrderService.Infrastructure.Repositories;

internal class MongoOrderRepository : IOrderRepository
{
    private readonly IMongoCollection<MongoOrder> _ordersCollection;
    private readonly IMapper _mapper;

    public MongoOrderRepository(IOptions<MongoDBSettings> settings, IMapper mapper)
    {
        _mapper = mapper;
        var client = new MongoClient(settings.Value.ConnectionURI);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _ordersCollection = database.GetCollection<MongoOrder>(settings.Value.CollectionName);
    }

    public async Task CreateAsync(OrderEntity order, CancellationToken cancellationToken = default)
    {
        var mongoOrder = _mapper.Map<MongoOrder>(order);

        await _ordersCollection.InsertOneAsync(mongoOrder, null, cancellationToken);
    }

    public async Task<List<OrderEntity>> ListWithPaginationAsync(int pageNo = 1, int pageSize = 10,
        CancellationToken cancellationToken = default,
        params Expression<Func<OrderEntity, bool>>[] filters)
    {
        var mongoFilters = new List<FilterDefinition<MongoOrder>>();

        foreach (var expression in filters)
        {
            var mongoExpression = _mapper.Map<Expression<Func<MongoOrder, bool>>>(expression);
            var mongoFilter = Builders<MongoOrder>.Filter.Where(mongoExpression);
            mongoFilters.Add(mongoFilter);
        }

        var combinedFilter = mongoFilters.Any()
            ? Builders<MongoOrder>.Filter.And(mongoFilters)
            : Builders<MongoOrder>.Filter.Empty;

        var orders = await _ordersCollection.Find(combinedFilter)
            .SortByDescending(order => order.CreatedAt)
            .Skip((pageNo - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<OrderEntity>>(orders);
    }

    public async Task<OrderEntity?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var order = await _ordersCollection.Find(order => order.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return _mapper.Map<OrderEntity>(order);
    }

    public async Task UpdateAsync(OrderEntity order, CancellationToken cancellationToken = default)
    {
        var filter = Builders<MongoOrder>.Filter.Where(o => o.Id == order.Id);
        var replaceOptions = new ReplaceOptions();
        var mongoOrder = _mapper.Map<MongoOrder>(order);

        await _ordersCollection.ReplaceOneAsync(filter, mongoOrder, replaceOptions, cancellationToken);
    }

    public async Task<long> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _ordersCollection.CountDocumentsAsync(Builders<MongoOrder>.Filter.Empty, null, cancellationToken);
    }

    public async Task<long> CountAsync(CancellationToken cancellationToken = default,
        params Expression<Func<OrderEntity, bool>>[] filters)
    {
        var mongoFilters = new List<FilterDefinition<MongoOrder>>();

        foreach (var expression in filters)
        {
            var mongoExpression = _mapper.Map<Expression<Func<MongoOrder, bool>>>(expression);
            var mongoFilter = Builders<MongoOrder>.Filter.Where(mongoExpression);
            mongoFilters.Add(mongoFilter);
        }

        var combinedFilter = mongoFilters.Any()
            ? Builders<MongoOrder>.Filter.And(mongoFilters)
            : Builders<MongoOrder>.Filter.Empty;

        return await _ordersCollection.CountDocumentsAsync(combinedFilter, null, cancellationToken);
    }
}
