using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductsService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<double>(type: "double", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CategoryProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryProducts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Percent = table.Column<int>(type: "int", nullable: false),
                    IsActived = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Discounts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "NormalizedName", "ParentId" },
                values: new object[,]
                {
                    { 1, "Smartphones", "smartphones", null },
                    { 2, "Laptops", "laptops", null }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "ImageUrl", "Name", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, null, "Samsung Galaxy S24 Ultra", 1119.99, 14 },
                    { 2, null, "IPhone 15 Pro", 999.0, 10 },
                    { 3, null, "MacBook Pro 16'", 2499.0, 2 },
                    { 4, null, "MacBook Air 13'", 1099.0, 12 }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "NormalizedName", "ParentId" },
                values: new object[,]
                {
                    { 3, "Brand", "smartphones_brand", 1 },
                    { 4, "Brand", "laptops_brand", 2 },
                    { 5, "RAM", "smartphones_ram", 1 },
                    { 6, "RAM", "laptops_ram", 2 },
                    { 7, "ROM", "smartphones_rom", 1 },
                    { 8, "ROM", "laptops_rom", 2 },
                    { 9, "Processor", "laptops_processor", 2 }
                });

            migrationBuilder.InsertData(
                table: "CategoryProducts",
                columns: new[] { "Id", "CategoryId", "ProductId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 8, 1, 2 },
                    { 15, 2, 3 },
                    { 24, 2, 4 }
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "Id", "EndDate", "IsActived", "Percent", "ProductId", "StartDate" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 15, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "NormalizedName", "ParentId" },
                values: new object[,]
                {
                    { 10, "Samsung", "smartphones_brand_samsung", 3 },
                    { 11, "Apple", "smartphones_brand_apple", 3 },
                    { 12, "12GB", "smartphones_ram_12gb", 5 },
                    { 13, "8GB", "smartphones_ram_8gb", 5 },
                    { 14, "512GB", "smartphones_rom_512gb", 7 },
                    { 15, "Apple", "laptops_brand_apple", 4 },
                    { 16, "16GB", "laptops_ram_16gb", 6 },
                    { 17, "18GB", "laptops_ram_18gb", 6 },
                    { 18, "512GB", "laptops_rom_512gb", 8 },
                    { 19, "1024GB", "laptops_ram_1024gb", 8 },
                    { 20, "M3", "laptops_processor_m3", 9 },
                    { 21, "M3 Pro", "laptops_processor_m3pro", 9 }
                });

            migrationBuilder.InsertData(
                table: "CategoryProducts",
                columns: new[] { "Id", "CategoryId", "ProductId" },
                values: new object[,]
                {
                    { 2, 3, 1 },
                    { 3, 5, 1 },
                    { 4, 7, 1 },
                    { 9, 3, 2 },
                    { 10, 5, 2 },
                    { 11, 7, 2 },
                    { 16, 4, 3 },
                    { 17, 6, 3 },
                    { 18, 8, 3 },
                    { 19, 9, 3 },
                    { 25, 4, 4 },
                    { 26, 6, 4 },
                    { 27, 8, 4 },
                    { 28, 9, 4 },
                    { 5, 10, 1 },
                    { 6, 12, 1 },
                    { 7, 14, 1 },
                    { 12, 11, 2 },
                    { 13, 13, 2 },
                    { 14, 14, 2 },
                    { 20, 15, 3 },
                    { 21, 17, 3 },
                    { 22, 19, 3 },
                    { 23, 21, 3 },
                    { 29, 15, 4 },
                    { 30, 16, 4 },
                    { 31, 18, 4 },
                    { 32, 20, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                table: "Categories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProducts_CategoryId",
                table: "CategoryProducts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProducts_ProductId",
                table: "CategoryProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_ProductId",
                table: "Discounts",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryProducts");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
