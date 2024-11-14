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
	private readonly IMongoCollection<Order> _ordersCollection;

	public MongoDBService(IOptions<MongoDBSettings> settings, IConfiguration configuration)
	{
		var client = new MongoClient(settings.Value.ConnectionURI);
		var database = client.GetDatabase(settings.Value.DatabaseName);
		_ordersCollection = database.GetCollection<Order>(settings.Value.CollectionName);
	}

	public async Task CreateOrderAsync(Order order)
	{
		await _ordersCollection.InsertOneAsync(order);
	}

	public async Task<Order?> GetOrderByIdAsync(string id)
	{
		var order = await _ordersCollection.Find(order => order.Id == id).FirstOrDefaultAsync();
		return order;
	}

	public async Task<PaginatedListModel<Order>> ListOrdersWithPaginationAsync(int pageNo = 1, int pageSize = 10, 
		params Expression<Func<Order, bool>>[] filters)
	{
		var mongoFilters = new List<FilterDefinition<Order>>();

		foreach (var filter in filters)
		{
			var mongoFilter = Builders<Order>.Filter.Where(filter);
			mongoFilters.Add(mongoFilter);
		}

		var combinedFilter = mongoFilters.Any()
			? Builders<Order>.Filter.And(mongoFilters)
			: Builders<Order>.Filter.Empty;

		var orders = await _ordersCollection.Find(combinedFilter)
			.SortByDescending(order => order.CreatedDate)
			.Skip((pageNo - 1) * pageSize)
			.Limit(pageSize)
			.ToListAsync();

		var count = await _ordersCollection.CountDocumentsAsync(combinedFilter);

		var data = new PaginatedListModel<Order>
		{
			Items = orders,
			CurrentPage = pageNo,
			TotalPages = (int)Math.Ceiling(count / (double)pageSize)
		};

		return data;
	}

	public async Task UpdateOrderAsync(Order order)
	{
		var filter = Builders<Order>.Filter.Where(o => o.Id == order.Id);
		await _ordersCollection.ReplaceOneAsync(filter, order);
	}
}
