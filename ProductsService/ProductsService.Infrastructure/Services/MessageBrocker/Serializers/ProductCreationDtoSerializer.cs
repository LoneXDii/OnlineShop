using Confluent.Kafka;
using ProductsService.Infrastructure.Models;
using System.Text;
using System.Text.Json;

namespace ProductsService.Infrastructure.Services.MessageBrocker.Serializers;

internal class ProductCreationDtoSerializer : ISerializer<ProductCreationDTO>
{
    public byte[] Serialize(ProductCreationDTO data, SerializationContext context)
    {
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
    }
}
