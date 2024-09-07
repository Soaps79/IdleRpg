using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/CharacterSheet")]
public class CharacterSheet : ScriptableObject
{
	public Sprite AvatarImage;
	public string CharacterName;
	public int BaseHealth;
	public ActiveSkill StartingSkill;
}
