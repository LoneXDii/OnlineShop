namespace UserService.DAL.Models;

public class ConsumedOrder
{
    public string UserEmail { get; set; } = "";
    public string? Id { get; set; }
    public int OrderStatus { get; set; }
    public string UserId { get; set; }
    public double TotalPrice { get; set; }
    public List<ConsumedProduct> Products { get; set; }
}
