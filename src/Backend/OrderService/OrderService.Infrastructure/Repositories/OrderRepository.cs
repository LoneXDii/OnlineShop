using AutoMapper;
using MongoDB.Driver;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Models;
using System.Linq.Expressions;

namespace OrderService.Infrastructure.Repositories;

internal class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<Order> _ordersCollection;
    private readonly IMapper _mapper;

    public OrderRepository(IMongoCollection<Order> ordersCollection, IMapper mapper)
    {
        _mapper = mapper;
        _ordersCollection = ordersCollection;
    }

    public async Task CreateAsync(OrderEntity order, CancellationToken cancellationToken = default)
    {
        var mongoOrder = _mapper.Map<Order>(order);

        await _ordersCollection.InsertOneAsync(mongoOrder, null, cancellationToken);
    }

    public async Task<List<OrderEntity>> ListWithPaginationAsync(int pageNo = 1, int pageSize = 10,
        CancellationToken cancellationToken = default,
        params Expression<Func<OrderEntity, bool>>[] filters)
    {
        var filter = CreateFilter(filters);

        var orders = await _ordersCollection.Find(filter)
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
        var filter = Builders<Order>.Filter.Where(o => o.Id == order.Id);
        var replaceOptions = new ReplaceOptions();
        var mongoOrder = _mapper.Map<Order>(order);

        await _ordersCollection.ReplaceOneAsync(filter, mongoOrder, replaceOptions, cancellationToken);
    }

    public async Task<long> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _ordersCollection.CountDocumentsAsync(Builders<Order>.Filter.Empty, null, cancellationToken);
    }

    public async Task<long> CountAsync(CancellationToken cancellationToken = default,
        params Expression<Func<OrderEntity, bool>>[] filters)
    {
        var filter = CreateFilter(filters);

        return await _ordersCollection.CountDocumentsAsync(filter, null, cancellationToken);
    }

    private FilterDefinition<Order> CreateFilter(Expression<Func<OrderEntity, bool>>[] filters)
    {
        var mongoFilters = new List<FilterDefinition<Order>>();

        foreach (var expression in filters)
        {
            var mongoExpression = _mapper.Map<Expression<Func<Order, bool>>>(expression);
            var mongoFilter = Builders<Order>.Filter.Where(mongoExpression);
            mongoFilters.Add(mongoFilter);
        }

        return mongoFilters.Any()
            ? Builders<Order>.Filter.And(mongoFilters)
            : Builders<Order>.Filter.Empty;
    }
}
