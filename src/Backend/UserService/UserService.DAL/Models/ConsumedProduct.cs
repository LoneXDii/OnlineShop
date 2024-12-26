namespace UserService.DAL.Models;

public class ConsumedProduct
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public string? ImageUrl { get; set; }
    public int Discount { get; set; }
}
