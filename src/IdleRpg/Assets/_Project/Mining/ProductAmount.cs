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
			OnAmountChanged?.Invoke(this);
			_amount = value;
		}		
	}

	public Action<ProductAmount> OnAmountChanged;

	public ProductAmount() { }
	public ProductAmount(ProductSO product, int amount)
	{
		Product = product;
		Amount = amount;
	}
}