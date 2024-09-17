using QGame;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestTabController : TabController
{
	public override string TabName => "quest";
	public override string TabDisplayName => "Quest";

	[SerializeField]
	private QuestView _questViewPrefab;
	private QuestView _questView;
	
	public void InitializeQuest(QuestManager questManager)
	{
		_questView = Instantiate(_questViewPrefab, transform);
		_questView.Initialize(questManager, TabView);
	}

	protected override void OnInitialize() 	{ }
}