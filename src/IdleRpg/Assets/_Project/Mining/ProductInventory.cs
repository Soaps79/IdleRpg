using System.Collections.Generic;
using System;

[Serializable]
public class ProductInventory
{
	private Dictionary<ProductSO, int> _inventory = new Dictionary<ProductSO, int>();
	public ProductAmount[] CurrentInventory;

	public void AddProduct(ProductAmount oreAmount)
	{
		AddProduct(oreAmount.Product, oreAmount.Amount);
	}

	public void AddProduct(ProductSO product, int amount)
	{
		if (_inventory.ContainsKey(product))
		{
			_inventory[product] += amount;
		}
		else
		{
			_inventory.Add(product, amount);
		}
		SetDisplayInventory();
	}

	public void RemoveProduct(ProductSO product, int amount)
	{
		if (_inventory.ContainsKey(product))
		{
			_inventory[product] -= Math.Min(amount, _inventory[product]);
			if (_inventory[product] <= 0)
			{
				_inventory.Remove(product);
			}
		}
		SetDisplayInventory();
	}

	private void SetDisplayInventory()
	{
		var inventory = new List<ProductAmount>();
		foreach (var kvp in _inventory)
		{
			inventory.Add(new ProductAmount(kvp.Key, kvp.Value));
		}
		CurrentInventory = inventory.ToArray();
	}

	public List<ProductAmount> Purge()
	{
		var result = new List<ProductAmount>();
		foreach (var kvp in _inventory)
		{
			result.Add(new ProductAmount (kvp.Key, kvp.Value));
		}
		_inventory.Clear();
		SetDisplayInventory();
		return result;
	}

	public int GetCurrentAmount(ProductSO product)
	{
		if (_inventory.ContainsKey(product))
		{
			return _inventory[product];
		}
		return 0;
	}
}