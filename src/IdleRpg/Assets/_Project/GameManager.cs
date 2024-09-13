using UnityEngine;
using QGame;

public class GameManager : QScript
{
	[SerializeField]
	private CharacterSheet[] _characters;

	[SerializeField]
	private QuestSo _quest;

	[SerializeField]
	private BattleManager _battleManager;
	[SerializeField]
	private QuestManager _questManager;
	[SerializeField]
	private UiManager _uiManager;

	private void Start()
	{
		OnNextUpdate += BeginGame;
	}

	private void BeginGame()
	{
		_questManager.Initialize(_battleManager, _characters);
		_questManager.BeginQuest(_quest);

		_uiManager.QuestTabController.InitializeQuest(_questManager);		
	}
}