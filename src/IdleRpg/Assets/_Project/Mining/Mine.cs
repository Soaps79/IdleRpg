using UnityEngine;
using QGame;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;
using System.Linq;

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

    public List<OreAmount> GetOre(int amount)
    {
        var result = new List<OreAmount>();
		var remainingAmount = amount;

        foreach (var ore in _inventory.CurrentInventory.ToList())
        {
			if (ore.Amount >= remainingAmount)
            {
				_inventory.RemoveOre(ore.OreType, amount);
				result.Add(new OreAmount(ore.OreType, amount));
				remainingAmount = 0;
			}
			else
            {
				_inventory.RemoveOre(ore.OreType, ore.Amount);
				result.Add(new OreAmount(ore.OreType, ore.Amount));
				remainingAmount -= ore.Amount;
			}
			if (remainingAmount == 0)
				break;
			if (remainingAmount < 0)
				throw new Exception("Mine ore removal algo went negative");
		}

		return result;
    }
}