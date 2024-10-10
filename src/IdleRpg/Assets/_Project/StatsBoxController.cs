using UnityEngine;
using QGame;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;

public class StatsBoxController : QScript
{
	private const string _topStatsName = "top-stats";
	private const string _intentoryContainerName = "inventory-container";

	private List<SingleItemView> _topItems = new List<SingleItemView>();
	private List<SingleItemView> _bottomItems = new List<SingleItemView>();

	private CoreInventory _coreInventory;

	[SerializeField]
	private VisualTreeAsset _statsBoxTemplate;

	private TemplateContainer _statsBox;
	private VisualElement _topstats;
	private VisualElement _bottomInventoryContainer;

	private void Start()
	{
		_coreInventory = Locator.CoreInventory;
		var statsBoxContainer = Locator.UIManager.StatsBoxContainer;

		_statsBox = _statsBoxTemplate.Instantiate();
		_topstats = _statsBox.Q<VisualElement>(_topStatsName);
		_bottomInventoryContainer = _statsBox.Q<VisualElement>(_intentoryContainerName);

		_statsBox.style.flexGrow = 1;
		statsBoxContainer.Add(_statsBox);
		OnNextUpdate += () => {
			WireUpTopStats();
			WireUpBottomStats();
		};
	}

	private void WireUpTopStats()
	{
		foreach (var productAmount in _coreInventory.Products.GetAll())
		{
			if (productAmount.Product.SortOrder > 30)
				break;

			var singleItemView = new SingleItemView(Locator.UIManager.SingleValueTemplate);
			var view = singleItemView.Bind(productAmount, false, true, false);
			_topstats.Add(view);
			_topItems.Add(singleItemView);
		}
	}

	private void WireUpBottomStats()
	{
		foreach (var productAmount in _coreInventory.Products.GetAll())
		{
			if (productAmount.Product.SortOrder <= 30)
				continue;

			var singleItemView = new SingleItemView(Locator.UIManager.SingleValueTemplate);
			var view = singleItemView.Bind(productAmount, false, true, false);
			_bottomInventoryContainer.Add(view);
			_bottomItems.Add(singleItemView);
		}
	}
}