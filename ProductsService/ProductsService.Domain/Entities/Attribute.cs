using ProductsService.Domain.Entities.Abstractions;

namespace ProductsService.Domain.Entities;

public class Attribute : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string NormalizedName { get; set; }
    public int? CategoryId { get; set; }
    public virtual Category? Category { get; set; }
    public virtual IEnumerable<Product>? Products { get; set; }
    public virtual IEnumerable<ProductAttribute> ProductAttributes { get; set; }
}
