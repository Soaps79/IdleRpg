using UnityEngine;
using QGame;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class UiManager : QScript
{
	private string _tabNameFormat = "button-";
	private const string _viewTabName = "view-container";
	private const string _tabLabelName = "tab-name-label";

	// document that contains the whole game UI
	[SerializeField]
	private UIDocument _wholeScreenDocument;
	// window that holds all tab views
	private VisualElement _mainWindow;

	// prefab for container for all tab views
	[SerializeField]
	private VisualTreeAsset _tabViewTemplate;

	private List<TabController> _tabControllers = new List<TabController>();

	public QuestTabController QuestTabController { get; private set; }

	// place all tabs in here to be considered in-game
	public TabController[] Tabs;

	private void Start()
	{
		_mainWindow = _wholeScreenDocument.rootVisualElement.Q<VisualElement>(GameUiNames.MainWindow);

		foreach (var tab in Tabs)
		{
			// only tabe who have existing buttons will be created
			var button = _wholeScreenDocument.rootVisualElement.Q<Button>(_tabNameFormat + tab.TabName);
			if(button == null)
			{
				Debug.LogError("No button found for tab: " + tab.TabName);
				continue;
			}

			// instantiate a tab view container, name it, and hand it to the tab controller
			var tabView = CreateTabView();
			var label = tabView.Q<Label>(_tabLabelName);
			label.text = tab.TabDisplayName;
			_mainWindow.Add(tabView);
			tab.Initialize(tabView);

			// add button event to show tab view
			button.clicked += () => HandleButtonPushed(tab.TabName);
			_tabControllers.Add(tab);
			button = null;

			// temp until quest flow is implemented
			if(tab is QuestTabController)
				QuestTabController = tab as QuestTabController;
		}

		OnNextUpdate += () =>
		{
			HandleButtonPushed("party");
		};
	}

	private void HandleButtonPushed(string name)
	{
		foreach (var tab in _tabControllers)
		{
			tab.SetVisible(name == tab.TabName ? true : false);
		}
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