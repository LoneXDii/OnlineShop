using Confluent.Kafka;
using OrderService.Infrastructure.Models;
using System.Text;
using System.Text.Json;

namespace OrderService.Infrastructure.Services.MessageBrocker.Consumers.Deserealizers;

internal class ConsumedUserDeserealizer : IDeserializer<ConsumedUser>
{
    public ConsumedUser Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        var jsonString = Encoding.UTF8.GetString(data);

        return JsonSerializer.Deserialize<ConsumedUser>(jsonString);
    }
}
