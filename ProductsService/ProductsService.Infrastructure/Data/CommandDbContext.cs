using Microsoft.EntityFrameworkCore;
using ProductsService.Domain.Entities;
using Attribute = ProductsService.Domain.Entities.Attribute;

namespace ProductsService.Infrastructure.Data;

internal class CommandDbContext : DbContext
{
    public CommandDbContext(DbContextOptions<CommandDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attribute>()
            .HasOne(attribute => attribute.Category)
            .WithMany(category => category.Attributes)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Product>()
            .HasOne(product => product.Category)
            .WithMany(category => category.Products)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Product>()
            .HasMany(product => product.Attributes)
            .WithMany(attribute => attribute.Products)
            .UsingEntity<ProductAttribute>();

        modelBuilder.Entity<Discount>()
            .HasOne(discount => discount.Product)
            .WithMany(product => product.Discounts)
            .OnDelete(DeleteBehavior.SetNull);

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
            }
        );

        modelBuilder.Entity<Attribute>().HasData(
            new Attribute
            {
                Id = 1,
                Name = "Brand",
                NormalizedName = "phone_brand",
                CategoryId = 1
            },
            new Attribute
            {
                Id = 2,
                Name = "Brand",
                NormalizedName = "laptop_brand",
                CategoryId = 2
            },
            new Attribute
            {
                Id = 3,
                Name = "RAM",
                NormalizedName = "phone_ram",
                CategoryId = 1
            },
            new Attribute
            {
                Id = 4,
                Name = "RAM",
                NormalizedName = "laptop_ram",
                CategoryId = 2
            },
            new Attribute
            {
                Id = 5,
                Name = "ROM",
                NormalizedName = "phone_rom",
                CategoryId = 1
            },
            new Attribute
            {
                Id = 6,
                Name = "ROM",
                NormalizedName = "laptop_rom",
                CategoryId = 2
            },
            new Attribute
            {
                Id = 7,
                Name = "Processor",
                NormalizedName = "laptop_processor",
                CategoryId = 2
            }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Samsung Galaxy S24 Ultra",
                Price = 1119.99,
                Quantity = 14,
                CategoryId = 1
            },
            new Product
            {
                Id = 2,
                Name = "IPhone 15 Pro",
                Price = 999,
                Quantity = 10,
                CategoryId = 1
            },
            new Product
            {
                Id = 3,
                Name = "MacBook Pro 16'",
                Price = 2499,
                Quantity = 2,
                CategoryId = 2
            },
            new Product
            {
                Id = 4,
                Name = "MacBook Air 13'",
                Price = 1099,
                Quantity = 12,
                CategoryId = 2
            }
        );

        modelBuilder.Entity<ProductAttribute>().HasData(
            new ProductAttribute
            {
                Id = 1,
                ProductId = 1,
                AttributeId = 1,
                Value = "Samsung"
            },
            new ProductAttribute
            {
                Id = 2,
                ProductId = 1,
                AttributeId = 3,
                Value = "12GB"
            },
            new ProductAttribute
            {
                Id = 3,
                ProductId = 1,
                AttributeId = 5,
                Value = "512Gb"
            },
            new ProductAttribute
            {
                Id = 4,
                ProductId = 2,
                AttributeId = 1,
                Value = "Apple"
            },
            new ProductAttribute
            {
                Id = 5,
                ProductId = 2,
                AttributeId = 3,
                Value = "8GB"
            },
            new ProductAttribute
            {
                Id = 6,
                ProductId = 2,
                AttributeId = 5,
                Value = "512Gb"
            },
            new ProductAttribute
            {
                Id = 7,
                ProductId = 3,
                AttributeId = 2,
                Value = "Apple"
            },
            new ProductAttribute
            {
                Id = 8,
                ProductId = 3,
                AttributeId = 4,
                Value = "18GB"
            },
            new ProductAttribute
            {
                Id = 9,
                ProductId = 3,
                AttributeId = 6,
                Value = "1024Gb"
            },
            new ProductAttribute
            {
                Id = 10,
                ProductId = 3,
                AttributeId = 7,
                Value = "M3 Pro"
            },
            new ProductAttribute
            {
                Id = 11,
                ProductId = 4,
                AttributeId = 2,
                Value = "Apple"
            },
            new ProductAttribute
            {
                Id = 12,
                ProductId = 4,
                AttributeId = 4,
                Value = "16GB"
            },
            new ProductAttribute
            {
                Id = 13,
                ProductId = 4,
                AttributeId = 6,
                Value = "512Gb"
            },
            new ProductAttribute
            {
                Id = 14,
                ProductId = 4,
                AttributeId = 7,
                Value = "M3"
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
    public DbSet<Attribute> Attributes { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<ProductAttribute> ProductAttributes { get; set; }
}
