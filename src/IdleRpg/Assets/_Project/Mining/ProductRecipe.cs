using System;

[Serializable]
public class ProductRecipe
{
    public ProductAmount[] Ingredients;
    public ProductAmount[] Results;
    public float Duration;
}