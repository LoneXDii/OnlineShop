using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsService.Domain.Entities;

namespace ProductsService.Infrastructure.Data.Configuration;

internal class DiscountDataConfiguration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.HasData(
            new Discount
            {
                Id = 1,
                Percent = 15,
                ProductId = 3
            }
        );
    }
}
