using OrderService.Domain.Entities;

namespace OrderService.Domain.Abstractions.Data;

public interface ITemporaryStorageService
{
    public void SaveCart(Dictionary<int, ProductEntity> cart);
    public Dictionary<int, ProductEntity> GetCart();
}
