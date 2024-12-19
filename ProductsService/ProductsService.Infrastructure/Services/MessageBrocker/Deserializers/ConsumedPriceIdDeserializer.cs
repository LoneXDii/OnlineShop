using Confluent.Kafka;
using ProductsService.Infrastructure.Models;
using System.Text;
using System.Text.Json;

namespace ProductsService.Infrastructure.Services.MessageBrocker.Deserializers;

internal class ConsumedPriceIdDeserializer : IDeserializer<ConsumedPriceId>
{
    public ConsumedPriceId Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        var jsonString = Encoding.UTF8.GetString(data);

        return JsonSerializer.Deserialize<ConsumedPriceId>(jsonString);
    }
}
