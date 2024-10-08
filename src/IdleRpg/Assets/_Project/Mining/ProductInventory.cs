using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class ProductInventory
{
	private Dictionary<ProductSO, int> _inventory = new Dictionary<ProductSO, int>();
	public ProductAmount[] CurrentInventory;

	public Action<ProductAmount> OnAmountChanged;

	public bool LeaveEmptyElements;

	public ProductInventory() { }

	public void SetupAllProducts()
	{
		foreach (var product in Locator.Lookup.AllProducts)
		{
			AddProduct(product, 0);
		}
		SetDisplayInventory();
	}

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
		OnAmountChanged?.Invoke(new ProductAmount(product, amount));
	}

	// Remove functions currently assume that there is always enough product
	// will need work for partial results if needed
	public void RemoveProduct(ProductAmount productAmount) 
	{
		RemoveProduct(productAmount.Product, productAmount.Amount);
	}

	public void RemoveProduct(ProductSO product, int amount)
	{
		if (_inventory.ContainsKey(product))
		{
			_inventory[product] -= Math.Min(amount, _inventory[product]);
			if (!LeaveEmptyElements && _inventory[product] <= 0)
			{
				_inventory.Remove(product);
			}
		}
		SetDisplayInventory();
		OnAmountChanged?.Invoke(new ProductAmount(product, -amount));
	}

	private void SetDisplayInventory()
	{
		var inventory = new List<ProductAmount>();
		foreach (var kvp in _inventory)
		{
			inventory.Add(new ProductAmount(kvp.Key, kvp.Value));
		}
		CurrentInventory = inventory.OrderBy(i => i.Product.SortOrder).ToArray();
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