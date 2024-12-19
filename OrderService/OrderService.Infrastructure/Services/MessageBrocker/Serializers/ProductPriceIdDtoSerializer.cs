using Confluent.Kafka;
using OrderService.Infrastructure.Models;
using System.Text;
using System.Text.Json;

namespace OrderService.Infrastructure.Services.MessageBrocker.Serializers;

internal class ProductPriceIdDtoSerializer : ISerializer<ProductPriceIdDTO>
{
    public byte[] Serialize(ProductPriceIdDTO data, SerializationContext context)
    {
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
    }
}
