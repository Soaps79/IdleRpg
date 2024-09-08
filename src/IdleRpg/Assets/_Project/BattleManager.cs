using QGame;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

// Party should be broken out next, given to participants

public class BattleManager : QScript
{
    private const string _playerPartyContainerName = "player-party";
    private const string _enemyPartyContainerName = "enemy-party";
    private const string _victoryLabelName = "victory-label";
    private const string _defeatLabelName = "defeat-label";

    private Label _victoryLabelElement;
    private Label _defeatLabelElement;

    public GameObject CharacterViewPrefab;
    public CharacterSheet[] Characters;
    public CharacterSheet[] Enemies;
    public VisualTreeAsset CharacterUiTemplate;

    public UIDocument BattleUiDocument;

    private int _lastId = 0;

    private BattleParty _playerParty = new();
    private BattleParty _enemyParty = new();

	private void Start()
	{
        
	}

	public void InitializeUi(UIDocument document)
	{
		BattleUiDocument.enabled = true;
		var mainWindow = document.rootVisualElement.Q<Box>(GameUiNames.MainWindow);
        mainWindow.Add(BattleUiDocument.rootVisualElement);
        Initialize();
	}

	public void Initialize()
    {
        // find container for character views
        var playerPartyContainer = BattleUiDocument.rootVisualElement.Q<VisualElement>(_playerPartyContainerName);
		var enemyPartyContainer = BattleUiDocument.rootVisualElement.Q<VisualElement>(_enemyPartyContainerName);

        _victoryLabelElement = BattleUiDocument.rootVisualElement.Q<Label>(_victoryLabelName);
        _defeatLabelElement = BattleUiDocument.rootVisualElement.Q<Label>(_defeatLabelName);

		// for each character in Characters, init a view
		foreach (var character in Characters)
        {
            var obj = Instantiate(CharacterViewPrefab, transform);
                        
            var partipant = obj.GetComponent<BattleParticipant>();
            partipant.Initialize(character, _playerParty, _enemyParty);
            _lastId++;
            partipant.ParticipantId = _lastId;
			partipant.OnDeath += HandleParticipantDeath;

			var view = obj.GetComponent<BattleCharacterView>();
			var characterView = CharacterUiTemplate.Instantiate();
            view.BindToView(characterView);
            playerPartyContainer.Add(characterView);

            _playerParty.AddParticipant(partipant);
        }

		foreach (var character in Enemies)
		{
			var obj = Instantiate(CharacterViewPrefab, transform);

			var partipant = obj.GetComponent<BattleParticipant>();
			partipant.Initialize(character, _enemyParty, _playerParty);
			_lastId++;
			partipant.ParticipantId = _lastId;
			partipant.OnDeath += HandleParticipantDeath;

			var view = obj.GetComponent<BattleCharacterView>();
			var characterView = CharacterUiTemplate.Instantiate();
			view.BindToView(characterView);
			enemyPartyContainer.Add(characterView);

            _enemyParty.AddParticipant(partipant);
		}

		_playerParty.Begin();
        _enemyParty.Begin();
    }

	private void HandleParticipantDeath(BattleParticipant participant)
	{
		if(_playerParty.IsEveryoneDead())
        {
			_defeatLabelElement.visible = true;
		}
		else if(_enemyParty.IsEveryoneDead())
        {
			_victoryLabelElement.visible = true;
		}
	}
}