using ProductsService.Domain.Entities.Abstractions;

namespace ProductsService.Domain.Entities;

public class Product : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public string? ImageUrl { get; set; }
    public virtual Discount? Discount { get; set; }
    public virtual ICollection<Category>? Categories { get; set; }
}
