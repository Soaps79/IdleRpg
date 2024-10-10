using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class ProductInventory
{
	private Dictionary<ProductSO, ProductAmount> _inventory = new Dictionary<ProductSO, ProductAmount>();
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

	public List<ProductAmount> GetAll()
	{
		return _inventory.Values.OrderBy(i => i.Product.SortOrder).ToList();
	}

	public void AddProduct(ProductAmount oreAmount)
	{
		AddProduct(oreAmount.Product, oreAmount.Amount);
	}

	public void AddProduct(ProductSO product, int amount)
	{
		if (_inventory.ContainsKey(product))
		{
			_inventory[product].Amount += amount;
		}
		else
		{
			_inventory.Add(product, new ProductAmount(product, amount));
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
			_inventory[product].Amount -= Math.Min(amount, _inventory[product].Amount);
			if (!LeaveEmptyElements && _inventory[product].Amount <= 0)
			{
				_inventory.Remove(product);
			}
		}
		SetDisplayInventory();
		OnAmountChanged?.Invoke(new ProductAmount(product, -amount));
	}

	private void SetDisplayInventory()
	{
		CurrentInventory = _inventory.Values.OrderBy(i => i.Product.SortOrder).ToArray();
	}

	// does not yet handle leaving empty elements
	public List<ProductAmount> Purge()
	{
		var result = new List<ProductAmount>();
		foreach (var kvp in _inventory)
		{
			result.Add(kvp.Value);
		}
		_inventory.Clear();
		SetDisplayInventory();
		return result;
	}

	public int GetCurrentAmount(ProductSO product)
	{
		if (_inventory.ContainsKey(product))
		{
			return _inventory[product].Amount;
		}
		return 0;
	}
}