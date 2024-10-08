using UnityEngine;
using QGame;
using System.Linq;
using System.Collections.Generic;

public class CoreLookup : QScript
{
	public List<ProductSO> AllProducts { get; private set; }

	public void Awake()
	{
		ServiceLocator.Register<CoreLookup>(this);
		SetAllProducts();	
	}

	private void SetAllProducts()
	{
		ProductSO[] allProducts = (ProductSO[])Resources.FindObjectsOfTypeAll(typeof(ProductSO));
		Debug.Log("CoreLookup product count: " + allProducts.Length);
		AllProducts = allProducts.OrderBy(i => i.SortOrder).ToList();
	}
}