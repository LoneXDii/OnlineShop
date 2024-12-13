using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsService.Domain.Entities;

namespace ProductsService.Infrastructure.Data.Configuration;

internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasOne(category => category.Parent)
            .WithMany(category => category.Children)
            .HasForeignKey(category => category.ParentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(category => category.Products)
            .WithMany(product => product.Categories)
            .UsingEntity(j => j
                .HasData(
                    new { ProductsId = 1, CategoriesId = 1 },
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

        builder.HasData(
            new Category
            {
                Id = 1,
                Name = "Smartphones",
            },
            new Category
            {
                Id = 2,
                Name = "Laptops",
            },
            new Category
            {
                Id = 3,
                Name = "Brand",
                ParentId = 1,
            },
            new Category
            {
                Id = 4,
                Name = "Brand",
                ParentId = 2,
            },
            new Category
            {
                Id = 5,
                Name = "RAM",
                ParentId = 1,
            },
            new Category
            {
                Id = 6,
                Name = "RAM",
                ParentId = 2,
            },
            new Category
            {
                Id = 7,
                Name = "ROM",
                ParentId = 1,
            },
            new Category
            {
                Id = 8,
                Name = "ROM",
                ParentId = 2,
            },
            new Category
            {
                Id = 9,
                Name = "Processor",
                ParentId = 2,
            },
            new Category
            {
                Id = 10,
                Name = "Samsung",
                ParentId = 3,
            },
            new Category
            {
                Id = 11,
                Name = "Apple",
                ParentId = 3,
            },
            new Category
            {
                Id = 12,
                Name = "12GB",
                ParentId = 5,
            },
            new Category
            {
                Id = 13,
                Name = "8GB",
                ParentId = 5,
            },
            new Category
            {
                Id = 14,
                Name = "512GB",
                ParentId = 7,
            },
            new Category
            {
                Id = 15,
                Name = "Apple",
                ParentId = 4,
            },
            new Category
            {
                Id = 16,
                Name = "16GB",
                ParentId = 6,
            },
            new Category
            {
                Id = 17,
                Name = "18GB",
                ParentId = 6,
            },
            new Category
            {
                Id = 18,
                Name = "512GB",
                ParentId = 8,
            },
            new Category
            {
                Id = 19,
                Name = "1024GB",
                ParentId = 8,
            },
            new Category
            {
                Id = 20,
                Name = "M3",
                ParentId = 9,
            },
            new Category
            {
                Id = 21,
                Name = "M3 Pro",
                ParentId = 9,
            }
        );
    }
}
