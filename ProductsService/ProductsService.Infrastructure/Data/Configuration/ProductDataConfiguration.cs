using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsService.Domain.Entities;

namespace ProductsService.Infrastructure.Data.Configuration;

internal class ProductDataConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasData(
            new Product
            {
                Id = 1,
                Name = "Samsung Galaxy S24 Ultra",
                Price = 1119.99,
                Quantity = 14,
                PriceId = "price_1QXj0lCLnke0wpITkP8oVXC8"
            },
            new Product
            {
                Id = 2,
                Name = "IPhone 15 Pro",
                Price = 999,
                Quantity = 10,
                PriceId = "price_1QXj0mCLnke0wpITCVnosAnV"
            },
            new Product
            {
                Id = 3,
                Name = "MacBook Pro 16'",
                Price = 2499,
                Quantity = 2,
                PriceId = "price_1QXj0mCLnke0wpIT4M2mjScB"
            },
            new Product
            {
                Id = 4,
                Name = "MacBook Air 13'",
                Price = 1099,
                Quantity = 12,
                PriceId = "price_1QXj0nCLnke0wpITR1ZpsFfC"
            }
        );
    }
}
