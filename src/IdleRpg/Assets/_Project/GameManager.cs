using UnityEngine;
using QGame;
using UnityEngine.UIElements;

public class GameManager : QScript
{
    public UIDocument MainGameScreen;
	public TimedAction AutoAttack;

	public CharacterSheet[] Characters;

	public BattleCharacterView _battleCharacterView;

	public QuestSo Quest;

	public BattleManager BattleManager;
	public QuestManager QuestManager;

	public QuestView QuestView;
	public UIDocument QuestUiDocument;
	public UIDocument BattleUiDocument;

	[SerializeField]
	private UiManager _uiManager;

	private void Start()
	{
		OnNextUpdate += BeginGame;
	}

	private void BeginGame()
	{
		//InitializeBattleUi();

		QuestManager.Initialize(BattleManager, Characters);
		QuestManager.BeginQuest(Quest);

		_uiManager.QuestTabController.InitializeQuest(QuestManager);		
	}

	//public void InitializeBattleUi()
	//{
	//	BattleUiDocument.enabled = true;
	//	BattleUiDocument.rootVisualElement.visible = false;
	//	var mainWindow = MainGameScreen.rootVisualElement.Q<Box>(GameUiNames.MainWindow);
	//	mainWindow.Add(BattleUiDocument.rootVisualElement);
	//}

}