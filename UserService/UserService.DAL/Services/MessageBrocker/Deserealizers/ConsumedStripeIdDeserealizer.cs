using Confluent.Kafka;
using System.Text;
using System.Text.Json;
using UserService.DAL.Models;

namespace UserService.DAL.Services.MessageBrocker.Deserealizers;

internal class ConsumedStripeIdDeserealizer : IDeserializer<ConsumedStripeId>
{
    public ConsumedStripeId Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        var jsonString = Encoding.UTF8.GetString(data);

        return JsonSerializer.Deserialize<ConsumedStripeId>(jsonString);
    }
}
