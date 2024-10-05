using UnityEngine;
using QGame;
using UnityEngine.UIElements;
using System;
using Unity.VisualScripting;
using System.Linq;

public class MineView : QScript
{
    private string _nameLabelName = "name-label";
    private string _sliderName = "mine-slider";
    private string _oreContainerName = "ore-container";
    private string _singleOreName = "single-ore";

    private string _iconImageName = "icon-image";
    private string _oreNameLabelName = "ore-name-label";
    private string _amountLabelName = "amount-label";
    private string _invLabelName = "inv-label";

    private Label _nameLabel;
    private VisualElement _slider;
    private VisualElement _oreContainer;
    private VisualElement _singleOre;

    private SliderBinding _sliderBinding;

    private Mine _mine;

    public void Bind(VisualElement view, Mine mine)
    {
        _mine = mine;
		_mine.OnStateChanged += UpdateView;

        _nameLabel = view.Q<Label>(_nameLabelName);
		_slider = view.Q<VisualElement>(_sliderName);
		_oreContainer = view.Q<VisualElement>(_oreContainerName);
		_singleOre = view.Q<VisualElement>(_singleOreName);

		_sliderBinding = SliderBindingFactory.CreateSliderBinding(_slider, () => "", () => _mine.CraftingContainer.CurrentCraftProgress, transform);

     	UpdateView();
    }

	private void UpdateView()
	{
		_nameLabel.text = _mine.name;
		for (int i = 0; i < 3; i++)
		{
			var display = _oreContainer.Q<VisualElement>($"ore-{i}");

			if (i < _mine.Recipe.Results.Length)
			{
				display.Q<Label>(_oreNameLabelName).text = _mine.Recipe.Results[i].Name;
				display.Q<Label>(_amountLabelName).text = _mine.Recipe.Results[i].Amount.ToString();
				display.Q<Label>(_invLabelName).text = _mine.Inventory.GetCurrentAmount(_mine.Recipe.Results[i].Name).ToString();
			}
			else
			{
				display.style.display = DisplayStyle.None;
			}
		}
	}
}