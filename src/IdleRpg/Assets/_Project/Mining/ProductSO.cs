using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Product")]
public class ProductSO : ScriptableObject
{
    public string DisplayName;
    public string Name => name;
    public Sprite Icon;
    public Color IconTintColor;

    // SortOrder also serves as a representation of game progression
    // meaning the higher the SortOrder, the later in the game the product is unlocked
    // ie: When products are taken from Mines, the products with the highest SortOrder are taken first
    [Tooltip("1-30 Coins etc, 31-60 Ores")]
    public int SortOrder;
}