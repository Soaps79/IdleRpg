using UnityEngine;
using QGame;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class UiManager : QScript
{
	private string _tabNameFormat = "button-";
	private const string _viewTabName = "view-container";
	private const string _tabLabelName = "tab-name-label";

    [SerializeField]
    private UIDocument _wholeScreenDocument;
	[SerializeField]
	private VisualTreeAsset _tabViewTemplate;

    private List<TabController> _tabControllers = new List<TabController>();

	private VisualElement _mainWindow;

	public QuestTabController QuestTabController { get; private set; }

	public TabController[] Tabs;

	private void Start()
	{
		_mainWindow = _wholeScreenDocument.rootVisualElement.Q<VisualElement>(GameUiNames.MainWindow);

		foreach (var tab in Tabs)
		{
			var button = _wholeScreenDocument.rootVisualElement.Q<Button>(_tabNameFormat + tab.TabName);
			if(button == null)
			{
				Debug.LogError("No button found for tab: " + tab.TabName);
				continue;
			}
			var tabView = CreateTabView();
			var label = tabView.Q<Label>(_tabLabelName);
			label.text = "Quests";
			_mainWindow.Add(tabView);
			tab.Initialize(tabView);

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