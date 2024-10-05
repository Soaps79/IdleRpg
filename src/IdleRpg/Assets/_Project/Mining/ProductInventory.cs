using System.Collections.Generic;
using System;

[Serializable]
public class ProductInventory
{
	private Dictionary<string, int> _inventory = new Dictionary<string, int>();
	public ProductAmount[] CurrentInventory;

	public void AddProduct(ProductAmount oreAmount)
	{
		AddProduct(oreAmount.Name, oreAmount.Amount);
	}

	public void AddProduct(string productName, int amount)
	{
		if (_inventory.ContainsKey(productName))
		{
			_inventory[productName] += amount;
		}
		else
		{
			_inventory.Add(productName, amount);
		}
		SetDisplayInventory();
	}

	public void RemoveProduct(string productName, int amount)
	{
		if (_inventory.ContainsKey(productName))
		{
			_inventory[productName] -= Math.Min(amount, _inventory[productName]);
			if (_inventory[productName] <= 0)
			{
				_inventory.Remove(productName);
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

	public int GetCurrentAmount(string productName)
	{
		if (_inventory.ContainsKey(productName))
		{
			return _inventory[productName];
		}
		return 0;
	}
}