using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.DAL.Entities;

namespace UserService.DAL.Database.Configuration;

internal class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasData(
            new AppUser
            {
                Id = "67a62042-fd32-44c2-a4f0-258031063013",
                FirstName = "Admin",
                LastName = "Admin",
                UserName = "admin1@gmail.com",
                NormalizedUserName = "ADMIN1@GMAIL.COM",
                Email = "admin1@gmail.com",
                NormalizedEmail = "ADMIN1@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEAdaQZqBRnnJALCoMVEBzKIfyQ9gwxoKZrXtupQ7I1xmhYJyXzxID8aI/wEC8xKWpA==",
                SecurityStamp = "EKR7O7Q57LQH7N3LP3P7MW6NPMYMGMNF"
            },
            new AppUser
            {
                Id = "e925574a-ad03-4546-a23f-e0a16b1b1ecc",
                FirstName = "Customer",
                LastName = "Customer",
                UserName = "client1@gmail.com",
                NormalizedUserName = "CLIENT1@GMAIL.COM",
                Email = "client1@gmail.com",
                NormalizedEmail = "CLIENT1@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEKOnG0RxOt0J0HfVE3s644AzrV9RnvZqiaUy4AkiM7IgSP+zG5To41KpVwHM6oQzaQ==",
                SecurityStamp = "EC54SKVGI5LI43ZIJR5HB3CZHDLII2S6"
            }
        );
    }
}
