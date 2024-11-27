using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProductsService.Domain.Entities;

namespace ProductsService.Infrastructure.Data;

internal class CommandDbContext : DbContext
{
    public CommandDbContext(DbContextOptions<CommandDbContext> options) : base(options) { }

	public DbSet<Product> Products { get; set; }
	public DbSet<Category> Categories { get; set; }
	public DbSet<Discount> Discounts { get; set; }
    public DbSet<CategoryProduct> CategoryProducts { get; set; }

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

        modelBuilder.Entity<Product>()
            .HasMany(product => product.CategoryProducts)
            .WithOne(cp => cp.Product)
            .HasForeignKey(cp => cp.ProductId);

		modelBuilder.Entity<Category>()
			.HasMany(category => category.CategoryProducts)
			.WithOne(cp => cp.Category)
			.HasForeignKey(cp => cp.CategoryId);

		modelBuilder.Entity<CategoryProduct>()
			.HasData(
				new { Id = 1, ProductId = 1, CategoryId = 1 },
				new { Id = 2, ProductId = 1, CategoryId = 3 },
				new { Id = 3, ProductId = 1, CategoryId = 5 },
				new { Id = 4, ProductId = 1, CategoryId = 7 },
				new { Id = 5, ProductId = 1, CategoryId = 10 },
				new { Id = 6, ProductId = 1, CategoryId = 12 },
				new { Id = 7, ProductId = 1, CategoryId = 14 },
				new { Id = 8, ProductId = 2, CategoryId = 1 },
				new { Id = 9, ProductId = 2, CategoryId = 3 },
				new { Id = 10, ProductId = 2, CategoryId = 5 },
				new { Id = 11, ProductId = 2, CategoryId = 7 },
				new { Id = 12, ProductId = 2, CategoryId = 11 },
				new { Id = 13, ProductId = 2, CategoryId = 13 },
				new { Id = 14, ProductId = 2, CategoryId = 14 },
				new { Id = 15, ProductId = 3, CategoryId = 2 },
				new { Id = 16, ProductId = 3, CategoryId = 4 },
				new { Id = 17, ProductId = 3, CategoryId = 6 },
				new { Id = 18, ProductId = 3, CategoryId = 8 },
				new { Id = 19, ProductId = 3, CategoryId = 9 },
				new { Id = 20, ProductId = 3, CategoryId = 15 },
				new { Id = 21, ProductId = 3, CategoryId = 17 },
				new { Id = 22, ProductId = 3, CategoryId = 19 },
				new { Id = 23, ProductId = 3, CategoryId = 21 },
				new { Id = 24, ProductId = 4, CategoryId = 2 },
				new { Id = 25, ProductId = 4, CategoryId = 4 },
				new { Id = 26, ProductId = 4, CategoryId = 6 },
				new { Id = 27, ProductId = 4, CategoryId = 8 },
				new { Id = 28, ProductId = 4, CategoryId = 9 },
				new { Id = 29, ProductId = 4, CategoryId = 15 },
				new { Id = 30, ProductId = 4, CategoryId = 16 },
				new { Id = 31, ProductId = 4, CategoryId = 18 },
				new { Id = 32, ProductId = 4, CategoryId = 20 }
			);

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
}
