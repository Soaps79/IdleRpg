using QGame;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestTabController : QScript
{
	private VisualElement _mainWindow;

	[SerializeField]
	private QuestView _questViewPrefab;

	public void Initialize(UIDocument uIDocument)
	{
		_mainWindow = uIDocument.rootVisualElement.Q<VisualElement>(GameUiNames.MainWindow);
	}

	public void InitializeQuest(QuestManager questManager)
	{
		var questView = Instantiate(_questViewPrefab, transform);
		questView.Initialize(questManager, _mainWindow);
	}
}