using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.DAL.Migrations
{
    /// <inheritdoc />
    public partial class StripeIdAddedToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StripeId",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "67a62042-fd32-44c2-a4f0-258031063013",
                columns: new[] { "ConcurrencyStamp", "StripeId" },
                values: new object[] { "572209c6-4c8c-4a11-8c11-53754841c1b0", "cus_RQULukld15o0o1" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e925574a-ad03-4546-a23f-e0a16b1b1ecc",
                columns: new[] { "ConcurrencyStamp", "StripeId" },
                values: new object[] { "33e9a341-0a1f-465b-a959-fc6231799989", "cus_RQULrwlbuvAi6L" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "67a62042-fd32-44c2-a4f0-258031063013",
                column: "ConcurrencyStamp",
                value: "5343564c-b4e3-42e6-a69c-b595dedb9710");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e925574a-ad03-4546-a23f-e0a16b1b1ecc",
                column: "ConcurrencyStamp",
                value: "c9ad2f78-5c34-4899-b0fe-794b2f8e4a19");
        }
    }
}
