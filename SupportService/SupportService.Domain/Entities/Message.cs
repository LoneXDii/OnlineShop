using SupportService.Domain.Entities.Abstractions;

namespace SupportService.Domain.Entities;

public class Message : IEntity
{
    public int Id { get; set; }
    public string SenderId { get; set; }
    public string Text { get; set; }
    public DateTime DateTime { get; set; }
    public int ChatId { get; set; }
    public virtual Chat? Chat { get; set; }
}
