using ProductsService.Domain.Entities.Abstractions;

namespace ProductsService.Domain.Entities;

public class Category : IEntity
{
    public int Id { get; set; }
    public int Name { get; set; }
    public int NormalizedName { get; set; }
    public string? ImageUrl { get; set; }
    public IEnumerable<Attribute>? Attributes { get; set; }
}
