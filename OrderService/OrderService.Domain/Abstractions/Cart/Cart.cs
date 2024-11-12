using OrderService.Domain.Entities;

namespace OrderService.Domain.Abstractions.Cart;

public abstract class Cart
{
	public Dictionary<int, Product> Items { get; set; } = new();

	public int Count
	{
		get => Items.Sum(item => item.Value.Quantity);
	}

	public double TotalCost
	{
		get => Items.Sum(item => item.Value.Price * item.Value.Quantity);
	}

	public virtual void AddToCart(Product product)
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

	public virtual void ReduceInCart(int id, int quantity)
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
