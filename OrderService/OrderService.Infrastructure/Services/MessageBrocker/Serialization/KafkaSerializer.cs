﻿using Confluent.Kafka;
using System.Text;
using System.Text.Json;

namespace OrderService.Infrastructure.Services.MessageBrocker.Serialization;

internal class KafkaSerializer<T> : ISerializer<T>
{
    public byte[] Serialize(T data, SerializationContext context)
    {
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
    }
}
