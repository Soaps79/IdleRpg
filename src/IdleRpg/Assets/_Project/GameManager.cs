using UnityEngine;
using QGame;
using System.Collections.Generic;
using System.Linq;

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

	[SerializeField]
	private InventorySO _startingInventory;

	private void Awake()
	{
		ServiceLocator.Register<GameManager>(this);
	}

	private void Start()
	{
		OnNextUpdate += BeginGame;
		if(_startingInventory != null)
		{
			_startingInventory.ApplyTo(Locator.CoreInventory.Products);
		}
	}

	private void BeginGame()
	{
		_questManager.Initialize(_battleManager, _characters);
		_questManager.BeginQuest(_quest);

		_uiManager.QuestTabController.InitializeQuest(_questManager);		
	}	
}