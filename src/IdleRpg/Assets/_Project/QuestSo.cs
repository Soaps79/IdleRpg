using UnityEngine;
using QGame;

[CreateAssetMenu(menuName = "Scriptables/Quest")]
public class QuestSo : ScriptableObject
{
    public Sprite MapSprite;
    public Color ProgressColor;

    public Sprite BattleBackground;
    public EnemyPartySo[] EnemyParties;

    public EnemyPartySo[] BossParty;
    public Sprite BossBackground;

    public float QuestLength;
    public float BattleCount;
}