using QGame;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestTabController : QScript
{
	private VisualElement _tabView;

	public void SetVisible(bool visible)
	{
		_tabView.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
	}

	[SerializeField]
	private QuestView _questViewPrefab;
	private QuestView _questView;

	public void Initialize(VisualElement element)
	{
		_tabView = element;
		SetVisible(false);
	}

	public void InitializeQuest(QuestManager questManager)
	{
		_questView = Instantiate(_questViewPrefab, transform);
		_questView.Initialize(questManager, _tabView);
	}
}