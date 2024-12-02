using Microsoft.EntityFrameworkCore;
using SupportService.Domain.Abstractions;
using SupportService.Domain.Entities;
using SupportService.Domain.Entities.Abstractions;
using SupportService.Infrastructure.Data;

namespace SupportService.Infrastructure.Repositories;

internal class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext, IRepository<Chat> chatRepository, IRepository<Message> messageRepository)
    {
        _dbContext = dbContext;
        ChatRepository = chatRepository;
        MessageRepository = messageRepository;
    }

    public IRepository<Chat> ChatRepository { get; private set; }
    public IRepository<Message> MessageRepository { get; private set; }

    public async Task EnableMigrationsAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.MigrateAsync(cancellationToken);
    }

    public async Task SaveAllAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
