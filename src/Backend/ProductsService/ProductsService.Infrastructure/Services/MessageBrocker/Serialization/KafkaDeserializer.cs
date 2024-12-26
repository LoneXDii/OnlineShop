using Confluent.Kafka;
using System.Text;
using System.Text.Json;

namespace ProductsService.Infrastructure.Services.MessageBrocker.Serialization;

internal class KafkaDeserializer<T> : IDeserializer<T>
{
    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        var jsonString = Encoding.UTF8.GetString(data);

        return JsonSerializer.Deserialize<T>(jsonString);
    }
}
