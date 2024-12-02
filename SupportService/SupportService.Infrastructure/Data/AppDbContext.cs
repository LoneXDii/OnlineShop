using Microsoft.EntityFrameworkCore;
using SupportService.Domain.Entities;

namespace SupportService.Infrastructure.Data;

internal class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>()
            .HasMany(chat => chat.Messages)
            .WithOne(message => message.Chat);
    }
}
