using UnityEngine;
using QGame;

[CreateAssetMenu(menuName = "Scriptables/EnemyParty")]
public class EnemyPartySo : ScriptableObject 
{
    public CharacterSheet[] Enemies;
    public int GoldReward;
    public int ExpReward;
}