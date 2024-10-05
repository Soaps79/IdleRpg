using System;
using System.Collections.Generic;

public enum OreTypes
{
	Copper,
	Lead,
	Iron,
	Silver,
	Gold
}

[Serializable]
public class OreRecipe
{
	public OreTypes OreType;
	public int OreAmount;
	public float TimeToCraft;
}

[Serializable]
public class OreAmount
{
	public OreTypes OreType;
	public int Amount;
	public OreAmount() { }
	public OreAmount(OreTypes oreType, int amount)
	{
		OreType = oreType;
		Amount = amount;
	}
}

[Serializable]
public class OreInventory
{
	private Dictionary<OreTypes, int> _inventory = new Dictionary<OreTypes, int>();
	public OreAmount[] CurrentInventory;

	public void AddOre(OreTypes oreType, int amount)
	{
		if (_inventory.ContainsKey(oreType))
		{
			_inventory[oreType] += amount;
		}
		else
		{
			_inventory.Add(oreType, amount);
		}
		SetInventory();
	}

	public void RemoveOre(OreTypes oreType, int amount)
	{
		if (_inventory.ContainsKey(oreType))
		{
			_inventory[oreType] -= Math.Min(amount, _inventory[oreType]);
			if (_inventory[oreType] <= 0)
			{
				_inventory.Remove(oreType);
			}
		}
		SetInventory();
	}

	private void SetInventory()
	{
		var inventory = new List<OreAmount>();
		foreach (var kvp in _inventory)
		{
			inventory.Add(new OreAmount { OreType = kvp.Key, Amount = kvp.Value });
		}
		CurrentInventory = inventory.ToArray();
	}

	public List<OreAmount> Purge()
	{
		var result = new List<OreAmount>();
		foreach (var kvp in _inventory)
		{
			result.Add(new OreAmount { OreType = kvp.Key, Amount = kvp.Value });
		}
		_inventory.Clear();
		return result;
	}
}