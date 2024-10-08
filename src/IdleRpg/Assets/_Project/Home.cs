using UnityEngine;
using QGame;

public class Home : QScript
{
    [SerializeField] 
    public ProductInventory Inventory;

	private void Start()
	{
		Inventory = Locator.CoreInventory.Products;
	}
}