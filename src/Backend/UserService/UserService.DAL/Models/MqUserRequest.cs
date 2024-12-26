using System.Text.Json;

namespace UserService.DAL.Models;

internal class MqUserRequest
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
}
