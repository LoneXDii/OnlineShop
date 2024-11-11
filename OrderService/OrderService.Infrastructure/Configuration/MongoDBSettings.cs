namespace OrderService.Infrastructure.Configuration;

internal class MongoDBSettings
{
	public string ConnectionURI { get; set; }
	public string DatabaseName { get; set; }
	public string CollectionName { get; set; }
}
