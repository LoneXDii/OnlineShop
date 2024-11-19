using ProductsService.Domain.Entities.Abstractions;

namespace ProductsService.Domain.Entities;

public class Category : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string NormalizedName { get; set; }
    public string? ImageUrl { get; set; }
    public virtual IEnumerable<Attribute>? Attributes { get; set; }
    public virtual IEnumerable<Product>? Products { get; set; }
}
