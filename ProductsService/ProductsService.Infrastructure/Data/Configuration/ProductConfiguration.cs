using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsService.Domain.Entities;

namespace ProductsService.Infrastructure.Data.Configuration;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasOne(product => product.Discount)
            .WithOne(discount => discount.Product)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
