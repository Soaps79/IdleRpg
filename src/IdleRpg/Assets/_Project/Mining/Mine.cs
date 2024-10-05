using UnityEngine;
using QGame;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class Mine : QScript, IPointerClickHandler
{
    [SerializeField]
    private OreRecipe[] _recipes;
	public OreRecipe[] Recipes { get { return _recipes; } }

    public OreInventory OreInventory = new OreInventory();

	public Action OnStateChanged;

	public CraftingContainer CraftingContainer { get; private set; }

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
            CraftingContainer = obj.GetOrAddComponent<CraftingContainer>();
			CraftingContainer.OnCraftComplete += OnCraftComplete;
			CraftingContainer.BeginCrafting(recipe);
		}
    }

	private void OnCraftComplete(OreRecipe recipe)
	{
		OreInventory.AddOre(recipe.OreType, recipe.OreAmount);
		OnStateChanged?.Invoke();
	}

    public List<OreAmount> GetOre(int amount)
    {
        var result = new List<OreAmount>();
		var remainingAmount = amount;

        foreach (var ore in OreInventory.CurrentInventory.ToList())
        {
			if (ore.Amount >= remainingAmount)
            {
				OreInventory.RemoveOre(ore.OreType, amount);
				result.Add(new OreAmount(ore.OreType, amount));
				remainingAmount = 0;
			}
			else
            {
				OreInventory.RemoveOre(ore.OreType, ore.Amount);
				result.Add(new OreAmount(ore.OreType, ore.Amount));
				remainingAmount -= ore.Amount;
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
		Debug.Log("Mine clicked");
		OnNextUpdate += () =>
		{
			Locator.UIManager.RequestMineOverlay(this);
		};
	}
}