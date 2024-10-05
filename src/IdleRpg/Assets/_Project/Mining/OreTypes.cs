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