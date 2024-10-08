using UnityEngine;
using QGame;

[CreateAssetMenu(menuName = "Scriptables/Inventory")]
public class InventorySO : ScriptableObject
{
    public ProductAmount[] Products;

    public void ApplyTo(ProductInventory inventory)
    {
		foreach (var product in Products)
        {
			inventory.AddProduct(product.Product, product.Amount);
		}
	}
}