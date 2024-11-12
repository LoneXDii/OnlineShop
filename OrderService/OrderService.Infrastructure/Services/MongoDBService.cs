using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using OrderService.Domain.Abstractions;
using OrderService.Domain.Models;
using OrderService.Infrastructure.Configuration;

namespace OrderService.Infrastructure.Services;

internal class MongoDBService : IDbService
{
	private readonly IMongoCollection<Order> _ordersCollection;

	public MongoDBService(IOptions<MongoDBSettings> settings)
	{
		MongoClient client = new MongoClient(settings.Value.ConnectionURI);
		IMongoDatabase database = client.GetDatabase(settings.Value.DatabaseName);
		_ordersCollection = database.GetCollection<Order>(settings.Value.CollectionName);
	}

	public async Task CreateOrderAsync(Order order)
	{
		await _ordersCollection.InsertOneAsync(order);
		return;
	}

	public async Task<List<Order>> GetOrdersListAsync()
	{
		var data = await _ordersCollection.Find(new BsonDocument()).ToListAsync();
		return data;
	}

	public async Task AddProductToOrderAsync(string orderId, Product product)
	{
		FilterDefinition<Order> filter = Builders<Order>.Filter.Eq("Id", orderId);
		UpdateDefinition<Order> update = Builders<Order>.Update.AddToSet("Products", product);
		await _ordersCollection.UpdateOneAsync(filter, update);
	}
}
