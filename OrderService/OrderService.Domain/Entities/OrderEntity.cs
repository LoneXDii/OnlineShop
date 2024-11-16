using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OrderService.Domain.Common.Statuses;

namespace OrderService.Domain.Entities;

public class OrderEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public OrderStatuses OrderStatus { get; set; } = OrderStatuses.Created;
    public PaymentStatuses PaymentStatus { get; set; } = PaymentStatuses.NotPaid;
    public string UserId { get; set; }
    public double TotalPrice { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<ProductEntity> Products { get; set; }
}
