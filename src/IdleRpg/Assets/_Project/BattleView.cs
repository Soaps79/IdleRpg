using UnityEngine;
using QGame;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;

public class BattleView : QScript
{
	private BattleManager _battleManager;

	private const string TopBoxName = "top-box";
	private const string BottomBoxName = "bottom-box";

	private VisualElement _playerPartyContainer;
	private VisualElement _enemyPartyContainer;

	private const string _victoryLabelName = "victory-label";
	private const string _defeatLabelName = "defeat-label";

	private Label _victoryLabelElement;
	private Label _defeatLabelElement;

	[SerializeField]
	private VisualTreeAsset CharacterUiTemplate;

	private List<VisualElement> _allCharacterViews = new List<VisualElement>();

	public void BeginBattle(BattleManager battleManager, VisualElement questView)
    {
		_battleManager = battleManager;
		_battleManager.OnBattleComplete += OnBattleComplete;
		_battleManager.OnVictory += () => _victoryLabelElement.visible = true;
		_battleManager.OnDefeat += () => _defeatLabelElement.visible = true;

		_enemyPartyContainer = questView.Q<VisualElement>(TopBoxName);
		_playerPartyContainer = questView.Q<VisualElement>(BottomBoxName);
		_victoryLabelElement = questView.Q<Label>(_victoryLabelName);
		_defeatLabelElement = questView.Q<Label>(_defeatLabelName);

		// for each character in Characters, init a view
		foreach (var participant in _battleManager.PlayerParty.Participants)
		{
			var view = participant.GetComponent<BattleCharacterView>();
			var characterView = CharacterUiTemplate.Instantiate();
			view.BindToView(characterView);
			_playerPartyContainer.Add(characterView);
			_allCharacterViews.Add(characterView);
		}

		foreach (var participant in _battleManager.EnemyParty.Participants)
		{
			var view = participant.GetComponent<BattleCharacterView>();
			var characterView = CharacterUiTemplate.Instantiate();
			view.BindToView(characterView);
			_enemyPartyContainer.Add(characterView);
			_allCharacterViews.Add(characterView);
		}
	}

	private void OnBattleComplete()
	{
		_victoryLabelElement.visible = false;
		_defeatLabelElement.visible = false;
		foreach (var characterView in _allCharacterViews)
		{
			characterView.RemoveFromHierarchy();
		}
	}
}