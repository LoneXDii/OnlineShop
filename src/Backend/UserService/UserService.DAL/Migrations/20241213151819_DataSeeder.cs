using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserService.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DataSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "93119a11-21ac-4520-9bb5-40e300c5be5a", null, "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AvatarUrl", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshTokenExpiresAt", "RefreshTokenValue", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "67a62042-fd32-44c2-a4f0-258031063013", 0, null, "5343564c-b4e3-42e6-a69c-b595dedb9710", "admin1@gmail.com", true, "Admin", "Admin", false, null, "ADMIN1@GMAIL.COM", "ADMIN1@GMAIL.COM", "AQAAAAIAAYagAAAAEAdaQZqBRnnJALCoMVEBzKIfyQ9gwxoKZrXtupQ7I1xmhYJyXzxID8aI/wEC8xKWpA==", null, false, null, null, "EKR7O7Q57LQH7N3LP3P7MW6NPMYMGMNF", false, "admin1@gmail.com" },
                    { "e925574a-ad03-4546-a23f-e0a16b1b1ecc", 0, null, "c9ad2f78-5c34-4899-b0fe-794b2f8e4a19", "client1@gmail.com", true, "Customer", "Customer", false, null, "CLIENT1@GMAIL.COM", "CLIENT1@GMAIL.COM", "AQAAAAIAAYagAAAAEKOnG0RxOt0J0HfVE3s644AzrV9RnvZqiaUy4AkiM7IgSP+zG5To41KpVwHM6oQzaQ==", null, false, null, null, "EC54SKVGI5LI43ZIJR5HB3CZHDLII2S6", false, "client1@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "93119a11-21ac-4520-9bb5-40e300c5be5a", "67a62042-fd32-44c2-a4f0-258031063013" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "93119a11-21ac-4520-9bb5-40e300c5be5a", "67a62042-fd32-44c2-a4f0-258031063013" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e925574a-ad03-4546-a23f-e0a16b1b1ecc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93119a11-21ac-4520-9bb5-40e300c5be5a");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "67a62042-fd32-44c2-a4f0-258031063013");
        }
    }
}
