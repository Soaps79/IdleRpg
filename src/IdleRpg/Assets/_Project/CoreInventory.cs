using UnityEngine;
using QGame;

// represents the inventory of the current player
public class CoreInventory : QScript
{
	public ProductInventory Products;

	private void Awake()
	{
		ServiceLocator.Register<CoreInventory>(this);		
		Products = new ProductInventory { LeaveEmptyElements = true };
	}

	private void Start()
	{
		Products.SetupAllProducts();
	}
}