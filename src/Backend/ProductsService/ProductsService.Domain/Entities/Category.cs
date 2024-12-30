using ProductsService.Domain.Entities.Abstractions;

namespace ProductsService.Domain.Entities;

public class Category : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ParentId { get; set; }
    public string? ImageUrl { get; set; }
    public virtual Category? Parent { get; set; }
    public virtual ICollection<Category>? Children { get; set;}
    public virtual ICollection<Product>? Products { get; set; }
}
