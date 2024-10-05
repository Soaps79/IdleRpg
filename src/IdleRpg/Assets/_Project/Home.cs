using UnityEngine;
using QGame;

public class Home : QScript
{
    [SerializeField] 
    private OreInventory _oreInventory;

	public void IntakeOre(OreTypes type, int amount)
	{
		_oreInventory.AddOre(type, amount);
	}
}