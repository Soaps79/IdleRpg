using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Product")]
public class ProductSO : ScriptableObject
{
    public string DisplayName;
    public string Name => name;
    public Sprite Icon;
    public Color IconTintColor;
}