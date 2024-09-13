using QGame;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

// Currently manages both Battle and its UI, should split out a View

public class BattleManager : QScript
{
    private const string _playerPartyContainerName = "player-party";
    private const string _enemyPartyContainerName = "enemy-party";
    private const string _victoryLabelName = "victory-label";
    private const string _defeatLabelName = "defeat-label";

    private Label _victoryLabelElement;
    private Label _defeatLabelElement;

    public GameObject CharacterViewPrefab;
    //public CharacterSheet[] Characters;
    //public CharacterSheet[] Enemies;
    public VisualTreeAsset CharacterUiTemplate;

    public UIDocument BattleUiDocument;

    private int _lastId = 0;

    private BattleParty _playerParty = new();
    private BattleParty _enemyParty = new();

	private VisualElement _playerPartyContainer;
	private VisualElement _enemyPartyContainer;

	private List<GameObject> _participants = new List<GameObject>();

	private void Start()
	{
        
	}

	//public void InitializeUi(UIDocument mainScreenDocument)
	//{
	//	BattleUiDocument.enabled = true;
	//	BattleUiDocument.rootVisualElement.visible = false;
	//	var mainWindow = mainScreenDocument.rootVisualElement.Q<Box>(GameUiNames.MainWindow);
 //       mainWindow.Add(BattleUiDocument.rootVisualElement);
	//}

	public void BeginBattle(CharacterSheet[] player, CharacterSheet[] enemy)
    {
        // find container for character views
        _playerPartyContainer = BattleUiDocument.rootVisualElement.Q<VisualElement>(_playerPartyContainerName);
		_enemyPartyContainer = BattleUiDocument.rootVisualElement.Q<VisualElement>(_enemyPartyContainerName);

        _victoryLabelElement = BattleUiDocument.rootVisualElement.Q<Label>(_victoryLabelName);
        _defeatLabelElement = BattleUiDocument.rootVisualElement.Q<Label>(_defeatLabelName);

		// for each character in Characters, init a view
		foreach (var character in player)
        {
            var obj = Instantiate(CharacterViewPrefab, transform);
			_participants.Add(obj);
                        
            var partipant = obj.GetComponent<BattleParticipant>();
            partipant.Initialize(character, _playerParty, _enemyParty);
            _lastId++;
            partipant.ParticipantId = _lastId;
			partipant.OnDeath += HandleParticipantDeath;

			var view = obj.GetComponent<BattleCharacterView>();
			var characterView = CharacterUiTemplate.Instantiate();
            view.BindToView(characterView);
            _playerPartyContainer.Add(characterView);

            _playerParty.AddParticipant(partipant);
        }

		foreach (var character in enemy)
		{
			var obj = Instantiate(CharacterViewPrefab, transform);
			_participants.Add(obj);

			var partipant = obj.GetComponent<BattleParticipant>();
			partipant.Initialize(character, _enemyParty, _playerParty);
			_lastId++;
			partipant.ParticipantId = _lastId;
			partipant.OnDeath += HandleParticipantDeath;

			var view = obj.GetComponent<BattleCharacterView>();
			var characterView = CharacterUiTemplate.Instantiate();
			view.BindToView(characterView);
			_enemyPartyContainer.Add(characterView);

            _enemyParty.AddParticipant(partipant);
		}

		BattleUiDocument.rootVisualElement.visible = true;

		_playerParty.Begin();
        _enemyParty.Begin();
    }

	private void HandleParticipantDeath(BattleParticipant participant)
	{
		if(_playerParty.IsEveryoneDead())
        {
			_defeatLabelElement.visible = true;
			BeginCompleteBattle();
		}
		else if(_enemyParty.IsEveryoneDead())
        {
			_victoryLabelElement.visible = true;
			BeginCompleteBattle();
		}
	}

	private void BeginCompleteBattle()
	{
		StopAllPartyActions();
		StopWatch.AddNode("done", 2.0f, true).OnTick += () =>
		{
			CompleteBattle();
		};
	}

	private void CompleteBattle()
	{
		foreach (var participant in _participants)
		{
			Destroy(participant);
		}

		_participants.Clear();

		_victoryLabelElement.visible = false;
		_defeatLabelElement.visible = false;
		_playerParty = new BattleParty();
		_enemyParty = new BattleParty();
		_playerPartyContainer.visible = false;
		_enemyPartyContainer.visible = false;
		OnBattleComplete?.Invoke();
	}

	private void StopAllPartyActions()
	{
		_playerParty.StopAllActions();
		_enemyParty.StopAllActions();
	}

	public Action OnBattleComplete;
}