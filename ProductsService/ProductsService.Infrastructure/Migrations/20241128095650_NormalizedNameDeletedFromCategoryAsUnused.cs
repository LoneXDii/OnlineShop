using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductsService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NormalizedNameDeletedFromCategoryAsUnused : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Categories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Categories",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "NormalizedName",
                value: "smartphones");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "NormalizedName",
                value: "laptops");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "NormalizedName",
                value: "smartphones_brand");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "NormalizedName",
                value: "laptops_brand");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "NormalizedName",
                value: "smartphones_ram");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "NormalizedName",
                value: "laptops_ram");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "NormalizedName",
                value: "smartphones_rom");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "NormalizedName",
                value: "laptops_rom");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9,
                column: "NormalizedName",
                value: "laptops_processor");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10,
                column: "NormalizedName",
                value: "smartphones_brand_samsung");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11,
                column: "NormalizedName",
                value: "smartphones_brand_apple");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12,
                column: "NormalizedName",
                value: "smartphones_ram_12gb");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 13,
                column: "NormalizedName",
                value: "smartphones_ram_8gb");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 14,
                column: "NormalizedName",
                value: "smartphones_rom_512gb");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 15,
                column: "NormalizedName",
                value: "laptops_brand_apple");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 16,
                column: "NormalizedName",
                value: "laptops_ram_16gb");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 17,
                column: "NormalizedName",
                value: "laptops_ram_18gb");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 18,
                column: "NormalizedName",
                value: "laptops_rom_512gb");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 19,
                column: "NormalizedName",
                value: "laptops_ram_1024gb");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 20,
                column: "NormalizedName",
                value: "laptops_processor_m3");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 21,
                column: "NormalizedName",
                value: "laptops_processor_m3pro");
        }
    }
}
