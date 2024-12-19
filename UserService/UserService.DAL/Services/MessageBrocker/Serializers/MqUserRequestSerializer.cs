using Confluent.Kafka;
using System.Text;
using System.Text.Json;
using UserService.DAL.Models;

namespace UserService.DAL.Services.MessageBrocker.Serializers;

internal class MqUserRequestSerializer : ISerializer<MqUserRequest>
{
    public byte[] Serialize(MqUserRequest data, SerializationContext context)
    {
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
    }
}
