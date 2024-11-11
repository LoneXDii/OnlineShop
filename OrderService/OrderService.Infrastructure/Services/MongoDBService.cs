using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderService.Domain.Abstractions;
using OrderService.Infrastructure.Configuration;
using OrderService.Infrastructure.MongoModels;

namespace OrderService.Infrastructure.Services;

internal class MongoDBService : IDbService
{
	private readonly IMongoCollection<MongoOrder> _ordersCollection;

	public MongoDBService(IOptions<MongoDBSettings> settings)
	{
		MongoClient client = new MongoClient(settings.Value.ConnectionURI);
		IMongoDatabase database = client.GetDatabase(settings.Value.DatabaseName);
		_ordersCollection = database.GetCollection<MongoOrder>(settings.Value.CollectionName);
	}
}
