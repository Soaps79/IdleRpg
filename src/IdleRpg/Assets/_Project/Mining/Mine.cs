using UnityEngine;
using QGame;
using Unity.VisualScripting;
using System;

public class Mine : QScript
{
    [SerializeField]
    private OreRecipe[] _recipes;

    [SerializeField]
    private OreInventory _inventory = new OreInventory();

	private void Start()
	{
        StartProducing();
	}

	public void StartProducing()
    {
        foreach (var recipe in _recipes)
        {
            var obj = new GameObject($"{recipe.OreType}-crafting-container");
            obj.transform.parent = transform;
            var container = obj.GetOrAddComponent<CraftingContainer>();
			container.OnCraftComplete += OnCraftComplete;
			container.BeginCrafting(recipe);
		}
    }

	private void OnCraftComplete(OreRecipe recipe)
	{
		_inventory.AddOre(recipe.OreType, recipe.OreAmount);
	}
}