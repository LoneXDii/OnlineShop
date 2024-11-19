using ProductsService.Domain.Entities.Abstractions;

namespace ProductsService.Domain.Entities;

public class Discount : IEntity
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Percent { get; set; }
    public bool IsActived { get; set; }
    public int? ProductId { get; set; }
    public virtual Product? Product { get; set; }
}
