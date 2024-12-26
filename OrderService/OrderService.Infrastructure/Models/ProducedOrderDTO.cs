using OrderService.Domain.Common.Statuses;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Models;

internal class ProducedOrderDTO
{
    public string UserEmail { get; set; } = "";
    public string? Id { get; set; }
    public OrderStatuses OrderStatus { get; set; } = OrderStatuses.Created;
    public string UserId { get; set; }
    public double TotalPrice { get; set; }
    public List<ProductEntity> Products { get; set; }
}
