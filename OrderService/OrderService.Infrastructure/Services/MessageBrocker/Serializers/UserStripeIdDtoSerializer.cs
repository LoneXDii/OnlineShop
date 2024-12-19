using Confluent.Kafka;
using OrderService.Infrastructure.Models;
using System.Text;
using System.Text.Json;

namespace OrderService.Infrastructure.Services.MessageBrocker.Serializers;

internal class UserStripeIdDtoSerializer : ISerializer<UserStripeIdDTO>
{
    public byte[] Serialize(UserStripeIdDTO data, SerializationContext context)
    {
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
    }
}
