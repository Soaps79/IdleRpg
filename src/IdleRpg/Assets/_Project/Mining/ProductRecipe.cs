using System;

[Serializable]
public class ProductAmount
{
    public ProductSO Product;
    public int Amount;

    public ProductAmount() { }
    public ProductAmount(ProductSO product, int amount)
    {
        Product = product;                                             
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