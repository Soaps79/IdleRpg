using UnityEngine;

public enum BattleStyle
{
	None,
	Support,
	Heal,
	Attack
}

[CreateAssetMenu(menuName = "Scriptables/CharacterSheet")]
public class CharacterSheet : ScriptableObject
{
	public Sprite AvatarImage;
	public string CharacterName;
	public int BaseHealth;
	public ActiveSkill[] Skills;
	public bool IsEnemy;
	public BattleStyle Style;
}
