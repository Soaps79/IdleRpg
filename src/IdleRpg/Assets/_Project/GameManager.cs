using UnityEngine;
using QGame;
using UnityEngine.UIElements;

public class GameManager : QScript
{
    public UIDocument MainGameScreen;
	public TimedAction AutoAttack;

	public CharacterSheet CharacterSheet;

	public BattleCharacterView _battleCharacterView;

	public BattleManager BattleManager;

	private void Start()
	{
		BattleManager.InitializeUi(MainGameScreen);
	}

}