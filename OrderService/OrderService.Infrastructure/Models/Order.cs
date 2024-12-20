﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OrderService.Domain.Common.Statuses;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Models;

public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public OrderStatuses OrderStatus { get; set; } = OrderStatuses.Created;
    public PaymentStatuses PaymentStatus { get; set; } = PaymentStatuses.NotPaid;
    public string UserId { get; set; }
    public double TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ProductEntity> Products { get; set; }
}