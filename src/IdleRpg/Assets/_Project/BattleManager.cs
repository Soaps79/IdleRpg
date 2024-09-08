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

    public GameObject CharacterViewPrefab;
    public CharacterSheet[] Characters;
    public CharacterSheet[] Enemies;
    public VisualTreeAsset CharacterUiTemplate;

    public UIDocument BattleUiDocument;

    private List<BattleParticipant> _playerParty = new();
	private List<BattleParticipant> _enemyParty = new();
    private Dictionary<int, int> _currentlyAttacking = new Dictionary<int, int>();

    private int _lastId = 0;

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

		// for each character in Characters, init a view
		foreach (var character in Characters)
        {
            var obj = Instantiate(CharacterViewPrefab, transform);
                        
            var partipant = obj.GetComponent<BattleParticipant>();
            partipant.Initialize(character, this);
            _lastId++;
            partipant.ParticipantId = _lastId;
            _currentlyAttacking.Add(_lastId, 0);

			var view = obj.GetComponent<BattleCharacterView>();
			var characterView = CharacterUiTemplate.Instantiate();
            view.BindToView(characterView);
            playerPartyContainer.Add(characterView);

            _playerParty.Add(partipant);
        }

		foreach (var character in Enemies)
		{
			var obj = Instantiate(CharacterViewPrefab, transform);

			var partipant = obj.GetComponent<BattleParticipant>();
			partipant.Initialize(character, this);
			_lastId++;
			partipant.ParticipantId = _lastId;
			_currentlyAttacking.Add(_lastId, 0);

			var view = obj.GetComponent<BattleCharacterView>();
			var characterView = CharacterUiTemplate.Instantiate();
			view.BindToView(characterView);
			enemyPartyContainer.Add(characterView);

            _enemyParty.Add(partipant);
		}

		foreach (var enemy in _enemyParty)
        {
            enemy.Begin();
        }

        foreach(var player in _playerParty)
        {
			player.Begin();
		}
    }

    public BattleParticipant GetNextTarget(BattleParticipant attacker)
    {
        BattleParticipant target = null;
        if (attacker.IsEnemy)
        {
            var alive = _playerParty.FindAll(p => p.CurrentHealth > 0);
			if (alive.Count > 0) 
                target = alive[Random.Range(0, alive.Count)];
        }
        else
        {
			var alive = _enemyParty.FindAll(p => p.CurrentHealth > 0);
            if(alive.Count > 0)
			    target = alive[Random.Range(0, alive.Count)];
		}

        _currentlyAttacking[attacker.ParticipantId] = target != null ? target.ParticipantId : 0;

        return target;
	}	
}