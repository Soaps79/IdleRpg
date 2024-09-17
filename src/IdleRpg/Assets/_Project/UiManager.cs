using UnityEngine;
using QGame;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System;

public class UiManager : QScript
{
    private const string _partyButtonName = "button-party";
    private const string _questButtonName = "button-quest";

	private const string _viewTabName = "view-container";
	private const string _tabLabelName = "tab-name-label";


    [SerializeField]
    private UIDocument _wholeScreenDocument;
	[SerializeField]
	private VisualTreeAsset _tabViewTemplate;

    [SerializeField]
    private QuestTabController _questTabController;
	[SerializeField]
	private PartyTabController _partyTabController;


	private VisualElement _mainWindow;

	public QuestTabController QuestTabController { get { return _questTabController; } }

	private void Start()
	{
		_mainWindow = _wholeScreenDocument.rootVisualElement.Q<VisualElement>(GameUiNames.MainWindow);
		
		var tabView = CreateTabView();
		var label = tabView.Q<Label>(_tabLabelName);
		label.text = "Quests";		
		_mainWindow.Add(tabView);
		_questTabController.Initialize(tabView);

		tabView = CreateTabView();
		label = tabView.Q<Label>(_tabLabelName);
		label.text = "Party";
		_mainWindow.Add(tabView);
		_partyTabController.Initialize(tabView);

		_wholeScreenDocument.rootVisualElement.Q<Button>(_partyButtonName).clicked += () => HandlePartyButtonClicked();
		_wholeScreenDocument.rootVisualElement.Q<Button>(_questButtonName).clicked += () => HandleQuestButtonClicked();

		OnNextUpdate += () =>
		{
			_partyTabController.SetVisible(true);
		};
	}

	private void HandlePartyButtonClicked()
	{
		_questTabController.SetVisible(false);
		_partyTabController.SetVisible(true);
	}

	private void HandleQuestButtonClicked()
	{
		_questTabController.SetVisible(true);
		_partyTabController.SetVisible(false);
	}

	private VisualElement CreateTabView()
	{
		VisualElement tabView = _tabViewTemplate.Instantiate();
		FixStyling(tabView);
		var container = tabView.Q<VisualElement>(_viewTabName);
		container.style.flexGrow = 1;
		return container;
	}

	// these are in its selector, no idea why they aren't applying
	private void FixStyling(VisualElement tabView)
	{
		var label = tabView.Q<Label>(_tabLabelName);
		label.style.unityTextAlign = TextAnchor.MiddleCenter;
		label.style.flexGrow = 1;
	}
}