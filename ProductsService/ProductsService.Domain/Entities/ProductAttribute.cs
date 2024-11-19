using ProductsService.Domain.Entities.Abstractions;

namespace ProductsService.Domain.Entities;

public class ProductAttribute : IEntity
{
    public int Id { get; set; }
    public string Value { get; set; }
    public int AttributeId { get; set; }
    public virtual Attribute? Attribute {  get; set; }
    public int ProductId { get; set; }
    public virtual Product? Product { get; set; }
}
