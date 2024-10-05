using UnityEngine;
using QGame;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class Mine : QScript, IPointerClickHandler
{
	public ProductRecipe Recipe;

    public ProductInventory Inventory = new ProductInventory();

	public Action OnStateChanged;

	public CraftingContainer CraftingContainer { get; private set; }

	private void Start()
	{
        StartProducing();
	}

	public void StartProducing()
    {
        var obj = new GameObject($"{name}-crafting-container");
        obj.transform.parent = transform;
        CraftingContainer = obj.GetOrAddComponent<CraftingContainer>();
		CraftingContainer.OnCraftComplete += OnCraftComplete;
		CraftingContainer.BeginCrafting(Recipe);
    }

	private void OnCraftComplete(ProductRecipe recipe)
	{
		foreach (var result in recipe.Results)
		{
			Inventory.AddProduct(result.Name, result.Amount);
		}
		OnStateChanged?.Invoke();
	}

    public List<ProductAmount> GetProducts(int amount)
    {
        var result = new List<ProductAmount>();
		var remainingAmount = amount;

        foreach (var product in Inventory.CurrentInventory.ToList())
        {
			if (product.Amount >= remainingAmount)
            {
				Inventory.RemoveProduct(product.Name, amount);
				result.Add(new ProductAmount(product.Name, amount));
				remainingAmount = 0;
			}
			else
            {
				Inventory.RemoveProduct(product.Name, product.Amount);
				result.Add(new ProductAmount(product.Name, product.Amount));
				remainingAmount -= product.Amount;
			}
			if (remainingAmount == 0)
				break;
			if (remainingAmount < 0)
				throw new Exception("Mine ore removal algo went negative");
		}

		if(result.Any(i => i.Amount > 0))
			OnStateChanged?.Invoke();

		return result;
    }

	public void OnPointerClick(PointerEventData eventData)
	{
		OnNextUpdate += () =>
		{
			Locator.UIManager.RequestMineOverlay(this);
		};
	}
}