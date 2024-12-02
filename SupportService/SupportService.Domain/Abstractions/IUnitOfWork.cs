using SupportService.Domain.Entities;

namespace SupportService.Domain.Abstractions;

public interface IUnitOfWork
{
    IRepository<Chat> ChatRepository { get; }
    IRepository<Message> MessageRepository { get; }

    Task SaveAllAsync();
    Task DeleteDataBaseAsync();
    Task EnableMigrationsAsync();
}
