using OrderService.Domain.Common.Statuses;
using OrderService.Domain.Entities;

namespace OrderService.Application.DTO;

public class OrderDTO
{
    public string? Id { get; set; }
	public OrderStatuses OrderStatus { get; set; }
	public PaymentStatuses PaymentStatus { get; set; }
	public string UserId { get; set; } = null!;
    public double TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ProductEntity> Products { get; set; } = null!;
}
