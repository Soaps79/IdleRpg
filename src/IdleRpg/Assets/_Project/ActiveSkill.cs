using UnityEngine;

public enum SkillType
{
	None,
	Support,
	Heal,
	Attack
}

[CreateAssetMenu(menuName = "Scriptables/ActiveSkill")]
public class ActiveSkill : ScriptableObject
{
    public string SkillName;
    public int CastTime;
    public int BaseDamageMin;
    public int BaseDamageMax;
	public bool IsAoe;
	public SkillType Type;
}