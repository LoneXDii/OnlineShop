using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OrderService.Domain.Models;

namespace OrderService.Infrastructure.MongoModels;

internal class MongoOrder : Order
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	new public string? Id { get; set; }
}
