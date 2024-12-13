using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserService.DAL.Database.Configuration;

internal class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole<string>
            {
                Id = "93119a11-21ac-4520-9bb5-40e300c5be5a",
                Name = "admin",
                NormalizedName = "ADMIN"
            }
        );
    }
}
