using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdminIdDeletedClientNameAddedToChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupportId",
                table: "Chats");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "Chats",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "Chats",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "Chats");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Chats",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "SupportId",
                table: "Chats",
                type: "int",
                nullable: true);
        }
    }
}
