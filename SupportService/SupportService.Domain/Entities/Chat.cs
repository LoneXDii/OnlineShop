using SupportService.Domain.Entities.Abstractions;

namespace SupportService.Domain.Entities;

public class Chat : IEntity
{
    public int Id { get; set; }
    public string ClientName { get; set; }
    public string ClientId { get; set; }
    public bool IsActive { get; set; } = true;
    public virtual ICollection<Message>? Messages { get; set; }
}
