using QGame;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleManager : QScript
{
    private const string _playerPartyContainerName = "player-party";
    private const string _enemyPartyContainerName = "enemy-party";

    public BattleCharacterView CharacterViewPrefab;
    public CharacterSheet[] Characters;
    public VisualTreeAsset CharacterUiDocument;
    public UIDocument BattleUiDocument;

	private void Start()
	{
        Initialize();
	}

	public void Initialize()
    {
        // find container for character views
        var playerPartyContainer = BattleUiDocument.rootVisualElement.Q<VisualElement>(_playerPartyContainerName);

        // for each character in Characters, init a view
        foreach (var character in Characters)
        {
            var view = Instantiate(CharacterViewPrefab);
            var characterView = CharacterUiDocument.Instantiate();
            view.BindToView(character, characterView);
            playerPartyContainer.Add(characterView);
        }
    }
}