using OrderService.Domain.Entities;

namespace OrderService.Domain.Abstractions.Cart;

public interface ICart
{
    int Count { get; }
    double TotalCost { get;  }
    Dictionary<int, ProductEntity> Items { get; }

    void AddToCart(ProductEntity product)
    {
        if (Items.ContainsKey(product.Id))
        {
            Items[product.Id].Quantity += product.Quantity;
        }
        else
        {
            Items.Add(product.Id, product);
        }
    }

    void ReduceInCart(int id, int quantity)
    {
        if (Items.ContainsKey(id))
        {
            Items[id].Quantity -= quantity;

            if (Items[id].Quantity < 0)
            {
                RemoveItems(id);
            }
        }
    }

    public virtual void RemoveItems(int id)
    {
        Items.Remove(id);
    }

    public virtual void ClearAll()
    {
        Items.Clear();
    }
}
