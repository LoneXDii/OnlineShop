using AutoFixture;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.Factories;

public static class EntityFactory
{
    private static readonly Fixture _fixture = new();

    public static Category CreateCategory(string? imageUrl = null)
    {
        return _fixture.Build<Category>()
            .With(c => c.ImageUrl, imageUrl)
            .Without(c => c.Parent)
            .Without(c => c.Products)
            .Without(c => c.Children)
            .Create();
    }

    public static Product CreateProduct(string? imageUrl = null)
    {
        return _fixture.Build<Product>()
            .With(p => p.ImageUrl, imageUrl)
            .Without(p => p.Categories)
            .Without(p => p.Discount)
            .Create();
    }
}