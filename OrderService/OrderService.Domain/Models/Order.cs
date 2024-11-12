using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OrderService.Domain.Models;

public class Order
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }
	public string OrderStatus { get; set; }
	public string PaymentStatus { get; set; }
	public string UserId { get; set; }
	public double TotalPrice { get; set; }
	public List<Product> Products { get; set; }
}
