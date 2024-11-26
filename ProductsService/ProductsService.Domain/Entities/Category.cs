using ProductsService.Domain.Entities.Abstractions;

namespace ProductsService.Domain.Entities;

public class Category : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string NormalizedName { get; set; }
    public int? ParentId { get; set; }
    public virtual Category? Parent { get; set; }
    public virtual IEnumerable<Category>? Children { get; set;}
    public virtual IEnumerable<Product>? Products { get; set; }
}
