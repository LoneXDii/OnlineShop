using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportService.Domain.Entities;

namespace SupportService.Infrastructure.Data.Configuration;

internal class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.HasMany(chat => chat.Messages)
            .WithOne(message => message.Chat);
    }
}
