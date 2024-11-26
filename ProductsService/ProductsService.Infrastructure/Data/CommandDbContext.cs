using Microsoft.EntityFrameworkCore;
using ProductsService.Domain.Entities;

namespace ProductsService.Infrastructure.Data;

internal class CommandDbContext : DbContext
{
    public CommandDbContext(DbContextOptions<CommandDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasOne(category => category.Parent)
            .WithMany(category => category.Children)
            .HasForeignKey(category => category.ParentId);

        modelBuilder.Entity<Discount>()
            .HasOne(discount => discount.Product)
            .WithMany(product => product.Discounts)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Category>()
            .HasMany(category => category.Products)
            .WithMany(product => product.Categories)
            .UsingEntity(j => j
                .HasData(
                    new { ProductsId = 1, CategoriesId =1 },
                    new { ProductsId = 1, CategoriesId = 3 },
                    new { ProductsId = 1, CategoriesId = 5 },
                    new { ProductsId = 1, CategoriesId = 7 },
                    new { ProductsId = 1, CategoriesId = 10 },
                    new { ProductsId = 1, CategoriesId = 12 },
                    new { ProductsId = 1, CategoriesId = 14 },
                    new { ProductsId = 2, CategoriesId = 1 },
                    new { ProductsId = 2, CategoriesId = 3 },
                    new { ProductsId = 2, CategoriesId = 5 },
                    new { ProductsId = 2, CategoriesId = 7 },
                    new { ProductsId = 2, CategoriesId = 11 },
                    new { ProductsId = 2, CategoriesId = 13 },
                    new { ProductsId = 2, CategoriesId = 14 },
                    new { ProductsId = 3, CategoriesId = 2 },
                    new { ProductsId = 3, CategoriesId = 4 },
                    new { ProductsId = 3, CategoriesId = 6 },
                    new { ProductsId = 3, CategoriesId = 8 },
                    new { ProductsId = 3, CategoriesId = 9 },
                    new { ProductsId = 3, CategoriesId = 15 },
                    new { ProductsId = 3, CategoriesId = 17 },
                    new { ProductsId = 3, CategoriesId = 19 },
                    new { ProductsId = 3, CategoriesId = 21 },
                    new { ProductsId = 4, CategoriesId = 2 },
                    new { ProductsId = 4, CategoriesId = 4 },
                    new { ProductsId = 4, CategoriesId = 6 },
                    new { ProductsId = 4, CategoriesId = 8 },
                    new { ProductsId = 4, CategoriesId = 9 },
                    new { ProductsId = 4, CategoriesId = 15 },
                    new { ProductsId = 4, CategoriesId = 16 },
                    new { ProductsId = 4, CategoriesId = 18 },
                    new { ProductsId = 4, CategoriesId = 20 }
                ));

        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Smartphones",
                NormalizedName = "smartphones"
            },
            new Category
            {
                Id = 2,
                Name = "Laptops",
                NormalizedName = "laptops"
            },
            new Category
            {
                Id = 3,
                Name = "Brand",
                NormalizedName = "smartphones_brand",
                ParentId = 1,
            },
            new Category
            {
                Id = 4,
                Name = "Brand",
                NormalizedName = "laptops_brand",
                ParentId = 2,
            },
            new Category
            {
                Id = 5,
                Name = "RAM",
                NormalizedName = "smartphones_ram",
                ParentId = 1,
            },
            new Category
            {
                Id = 6,
                Name = "RAM",
                NormalizedName = "laptops_ram",
                ParentId = 2,
            },
            new Category
            {
                Id = 7,
                Name = "ROM",
                NormalizedName = "smartphones_rom",
                ParentId = 1,
            },
            new Category
            {
                Id = 8,
                Name = "ROM",
                NormalizedName = "laptops_rom",
                ParentId = 2,
            },
            new Category
            {
                Id = 9,
                Name = "Processor",
                NormalizedName = "laptops_processor",
                ParentId = 2,
            },
            new Category
            {
                Id = 10,
                Name = "Samsung",
                NormalizedName = "smartphones_brand_samsung",
                ParentId = 3,
            },
            new Category
            {
                Id = 11,
                Name = "Apple",
                NormalizedName = "smartphones_brand_apple",
                ParentId = 3,
            },
            new Category
            {
                Id = 12,
                Name = "12GB",
                NormalizedName = "smartphones_ram_12gb",
                ParentId = 5,
            },
            new Category
            {
                Id = 13,
                Name = "8GB",
                NormalizedName = "smartphones_ram_8gb",
                ParentId = 5,
            },
            new Category
            {
                Id = 14,
                Name = "512GB",
                NormalizedName = "smartphones_rom_512gb",
                ParentId = 7,
            },
            new Category
            {
                Id = 15,
                Name = "Apple",
                NormalizedName = "laptops_brand_apple",
                ParentId = 4,
            },
            new Category
            {
                Id = 16,
                Name = "16GB",
                NormalizedName = "laptops_ram_16gb",
                ParentId = 6,
            },
            new Category
            {
                Id = 17,
                Name = "18GB",
                NormalizedName = "laptops_ram_18gb",
                ParentId = 6,
            },
            new Category
            {
                Id = 18,
                Name = "512GB",
                NormalizedName = "laptops_rom_512gb",
                ParentId = 8,
            },
            new Category
            {
                Id = 19,
                Name = "1024GB",
                NormalizedName = "laptops_ram_1024gb",
                ParentId = 8,
            },
            new Category
            {
                Id = 20,
                Name = "M3",
                NormalizedName = "laptops_processor_m3",
                ParentId = 9,
            },
            new Category
            {
                Id = 21,
                Name = "M3 Pro",
                NormalizedName = "laptops_processor_m3pro",
                ParentId = 9,
            }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Samsung Galaxy S24 Ultra",
                Price = 1119.99,
                Quantity = 14
            },
            new Product
            {
                Id = 2,
                Name = "IPhone 15 Pro",
                Price = 999,
                Quantity = 10
            },
            new Product
            {
                Id = 3,
                Name = "MacBook Pro 16'",
                Price = 2499,
                Quantity = 2
            },
            new Product
            {
                Id = 4,
                Name = "MacBook Air 13'",
                Price = 1099,
                Quantity = 12
            }
        );

        modelBuilder.Entity<Discount>().HasData(
            new Discount
            {
                Id = 1,
                Percent = 15,
                ProductId = 3
            }
        );
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Discount> Discounts { get; set; }
}
