using System;
using UnityEngine;

[Serializable]
public class ProductAmount
{
	public ProductSO Product;

	[SerializeField]
	private int _amount;

	public int Amount { 
		get { return _amount; }
		set
		{
			_amount = value;
			OnAmountChanged?.Invoke(this);
		}		
	}

	public Action<ProductAmount> OnAmountChanged;

	public ProductAmount() { }
	public ProductAmount(ProductSO product, int amount)
	{
		Product = product;
		Amount = amount;
	}

	public ProductAmount Clone()
	{
		return new ProductAmount(Product, Amount);
	}
}