namespace ProductsService.Application.DTO;

public class DiscountDTO
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Percent { get; set; }
}
