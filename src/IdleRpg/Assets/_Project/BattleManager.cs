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
    //private const string _playerPartyContainerName = "player-party";
    //private const string _enemyPartyContainerName = "enemy-party";
    //private const string _victoryLabelName = "victory-label";
    //private const string _defeatLabelName = "defeat-label";

    //private Label _victoryLabelElement;
    //private Label _defeatLabelElement;

    public GameObject CharacterViewPrefab;
    //public CharacterSheet[] Characters;
    //public CharacterSheet[] Enemies;
    //public VisualTreeAsset CharacterUiTemplate;

    //public UIDocument BattleUiDocument;

    private int _lastId = 0;

    public BattleParty PlayerParty { get; private set; } = new();
    public BattleParty EnemyParty { get; private set; } = new();

	//private VisualElement _playerPartyContainer;
	//private VisualElement _enemyPartyContainer;

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
  //      _playerPartyContainer = BattleUiDocument.rootVisualElement.Q<VisualElement>(_playerPartyContainerName);
		//_enemyPartyContainer = BattleUiDocument.rootVisualElement.Q<VisualElement>(_enemyPartyContainerName);

  //      _victoryLabelElement = BattleUiDocument.rootVisualElement.Q<Label>(_victoryLabelName);
  //      _defeatLabelElement = BattleUiDocument.rootVisualElement.Q<Label>(_defeatLabelName);

		// for each character in Characters, init a view
		foreach (var character in player)
        {
            var obj = Instantiate(CharacterViewPrefab, transform);
			_participants.Add(obj);
                        
            var partipant = obj.GetComponent<BattleParticipant>();
            partipant.Initialize(character, PlayerParty, EnemyParty);
            _lastId++;
            partipant.ParticipantId = _lastId;
			partipant.OnDeath += HandleParticipantDeath;

            PlayerParty.AddParticipant(partipant);
        }

		foreach (var character in enemy)
		{
			var obj = Instantiate(CharacterViewPrefab, transform);
			_participants.Add(obj);

			var partipant = obj.GetComponent<BattleParticipant>();
			partipant.Initialize(character, EnemyParty, PlayerParty);
			_lastId++;
			partipant.ParticipantId = _lastId;
			partipant.OnDeath += HandleParticipantDeath;

            EnemyParty.AddParticipant(partipant);
		}

		PlayerParty.Begin();
        EnemyParty.Begin();
    }

	private void HandleParticipantDeath(BattleParticipant participant)
	{
		if(PlayerParty.IsEveryoneDead())
        {
			OnDefeat?.Invoke();
			BeginCompleteBattle();
		}
		else if(EnemyParty.IsEveryoneDead())
        {
			OnVictory?.Invoke();
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

		//_victoryLabelElement.visible = false;
		//_defeatLabelElement.visible = false;
		PlayerParty = new BattleParty();
		EnemyParty = new BattleParty();
		//_playerPartyContainer.visible = false;
		//_enemyPartyContainer.visible = false;
		OnBattleComplete?.Invoke();
	}

	private void StopAllPartyActions()
	{
		PlayerParty.StopAllActions();
		EnemyParty.StopAllActions();
	}

	public Action OnBattleResult;
	public Action OnBattleComplete;
	public Action OnVictory;
	public Action OnDefeat;
}