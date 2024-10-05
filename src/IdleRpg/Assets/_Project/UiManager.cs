using UnityEngine;
using QGame;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Unity.VisualScripting;

public class UiManager : QScript
{
	private string _tabNameFormat = "button-";
	private const string _viewTabName = "view-container";
	private const string _tabLabelName = "tab-name-label";
	private const string _tooltipLayerName = "tooltip-layer";

	[SerializeField]
	private string _startingTab;

	// document that contains the whole game UI
	[SerializeField]
	private UIDocument _wholeScreenDocument;

	[SerializeField]
	private Vector2 _tooltipOffset;

	private VisualElement _root;

	// window that holds all tab views
	private VisualElement _mainWindow;

	// layer on which tooltips are created, captures events to clear them
	private VisualElement _tooltipLayer;

	// prefab for container for all tab views
	[SerializeField]
	private VisualTreeAsset _tabViewTemplate;

	// prefabs for tooltips - move these somewhere else once a pattern emerges
	[SerializeField]
	private VisualTreeAsset _mineTooltipTemplate;

	private List<TabController> _tabControllers = new List<TabController>();

	public QuestTabController QuestTabController { get; private set; }

	// place all tabs in here to be considered in-game
	public TabController[] Tabs;

	private void Awake()
	{
		ServiceLocator.Register<UiManager>(this);
	}

	private void Start()
	{
		_root = _wholeScreenDocument.rootVisualElement;
		_mainWindow = _wholeScreenDocument.rootVisualElement.Q<VisualElement>(GameUiNames.MainWindow);

		_tooltipLayer = _wholeScreenDocument.rootVisualElement.Q<VisualElement>(_tooltipLayerName);
		_tooltipLayer.RegisterCallback<ClickEvent>(evt => ClearTooltips());

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
			HandleButtonPushed(string.IsNullOrWhiteSpace(_startingTab) ? "party" : _startingTab);
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

	public void RequestMineOverlay(Mine mine)
	{
		ClearTooltips();

		var obj = new GameObject("mine-tooltip");
		var mineView = obj.GetOrAddComponent<MineView>();
		var tooltipView = _mineTooltipTemplate.Instantiate();
		tooltipView.style.flexGrow = 1;
		_tooltipLayer.Add(tooltipView);

		var panel = _root.panel;
		var pos = RuntimePanelUtils.CameraTransformWorldToPanel(panel, mine.transform.position, Camera.main);

		// TODO: Figure out offset
		// tooltipView.resolvedStyle.height should work but is not set until it is drawn
		tooltipView.style.top = pos.y;
		tooltipView.style.left = pos.x;

		mineView.Bind(tooltipView, mine);

		AllowTooltipClose();
	}

	private void ClearTooltips()
	{
		_tooltipLayer.Clear();
		_tooltipLayer.pickingMode = PickingMode.Ignore;
	}

	private void AllowTooltipClose()
	{
		_tooltipLayer.pickingMode = PickingMode.Position;
	}
}