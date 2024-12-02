using SupportService.Domain.Entities.Abstractions;

namespace SupportService.Domain.Entities;

public class Chat : IEntity
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int? SupportId { get; set; }
    public bool IsActive { get; set; }
}
