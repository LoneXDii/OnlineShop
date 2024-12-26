namespace OrderService.Domain.Entities;

public class ProductEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public string? ImageUrl { get; set; }
    public string PriceId { get; set; }
    public int Discount { get; set; }
}
