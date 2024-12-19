using Confluent.Kafka;
using OrderService.Infrastructure.Models;
using System.Text;
using System.Text.Json;

namespace OrderService.Infrastructure.Services.MessageBrocker.Deserializers;

internal class ConsumedUserDeserializer : IDeserializer<ConsumedUser>
{
    public ConsumedUser Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        var jsonString = Encoding.UTF8.GetString(data);

        return JsonSerializer.Deserialize<ConsumedUser>(jsonString);
    }
}
