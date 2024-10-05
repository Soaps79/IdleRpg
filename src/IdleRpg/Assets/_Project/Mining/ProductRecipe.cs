using System;

[Serializable]
public class ProductAmount
{
    public string Name;
    public int Amount;

    public ProductAmount() { }
    public ProductAmount(string name, int amount)
    {
		Name = name;
		Amount = amount;
	}
}

[Serializable]
public class ProductRecipe
{
    public ProductAmount[] Ingredients;
    public ProductAmount[] Results;
    public float Duration;
}