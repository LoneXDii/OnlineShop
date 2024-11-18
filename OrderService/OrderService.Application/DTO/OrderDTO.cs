using OrderService.Domain.Entities;

namespace OrderService.Application.DTO;

public class OrderDTO
{
    public string? Id { get; set; }
    public int OrderStatus { get; set; }
    public string OrderStatusDescription { get; set; } = null!;
    public int PaymentStatus { get; set; }
    public string PaymentStatusDescription { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public double TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ProductEntity> Products { get; set; } = null!;
}
