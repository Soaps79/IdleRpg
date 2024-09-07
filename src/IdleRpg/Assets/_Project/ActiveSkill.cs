using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/ActiveSkill")]
public class ActiveSkill : ScriptableObject
{
    public string SkillName;
    public int CastTime;
    public int BaseDamageMin;
    public int BaseDamageMax;
}