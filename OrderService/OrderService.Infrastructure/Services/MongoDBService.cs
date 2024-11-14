using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Common.Models;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Configuration;
using System.Linq.Expressions;

namespace OrderService.Infrastructure.Services;

internal class MongoDBService : IDbService
{
	private readonly IMongoCollection<OrderEntity> _ordersCollection;

	public MongoDBService(IOptions<MongoDBSettings> settings, IConfiguration configuration)
	{
		var client = new MongoClient(settings.Value.ConnectionURI);
		var database = client.GetDatabase(settings.Value.DatabaseName);
		_ordersCollection = database.GetCollection<OrderEntity>(settings.Value.CollectionName);
	}

	public async Task CreateOrderAsync(OrderEntity order)
	{
		await _ordersCollection.InsertOneAsync(order);
	}

	public async Task<OrderEntity?> GetOrderByIdAsync(string id)
	{
		var order = await _ordersCollection.Find(order => order.Id == id).FirstOrDefaultAsync();
		return order;
	}

	public async Task<PaginatedListModel<OrderEntity>> ListOrdersWithPaginationAsync(int pageNo = 1, int pageSize = 10, 
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
			.SortByDescending(order => order.CreatedDate)
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

	public async Task UpdateOrderAsync(OrderEntity order)
	{
		var filter = Builders<OrderEntity>.Filter.Where(o => o.Id == order.Id);
		await _ordersCollection.ReplaceOneAsync(filter, order);
	}
}
