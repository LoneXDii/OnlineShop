namespace ProductsService.Application.DTO;

public class ListProductsRequestDTO
{
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public double? PriceLessThan { get; set; }
    public double? PriceGreaterThan { get; set; }
    public int? CategoryId { get; set; }
}
