using UnityEngine;
using QGame;
using UnityEngine.UIElements;
using System;

public class SliderBinding : QScript
{
	public const string BarSliderLeftLabel = "label-left";
	public const string BarSliderRightLabel = "label-right";
	public const string BarSliderBar = "slider-bar";

	private VisualElement _skillSlider;
	private VisualElement _barSlider;

	private Label _leftLabel;
	private Label _rightLabel;
	private Func<float> _valueFunc;
	private Func<string> _leftTextFunc;

	public void Bind(VisualElement sliderElement, Func<string> leftTextFunc, Func<float> valueFunc)
	{
		_skillSlider = sliderElement;
		_barSlider = _skillSlider.Q<VisualElement>(BarSliderBar);
		_leftLabel = _skillSlider.Q<Label>(BarSliderLeftLabel);
		_rightLabel = _skillSlider.Q<Label>(BarSliderRightLabel);
		_valueFunc = valueFunc;
		_leftTextFunc = leftTextFunc;
	}

	protected override void OnUpdate()
	{
		_barSlider.style.width = new StyleLength(Length.Percent(_valueFunc() * 100));
		_leftLabel.text = _leftTextFunc();
		_rightLabel.text = $"{Math.Round(_valueFunc(), 2) * 100}%";
	}
}

public static class SliderBindingFactory
{
	public static SliderBinding CreateSliderBinding(VisualElement sliderElement, Func<string> leftTextFunc, Func<float> valueFunc, Transform parent)
	{
		var obj = new GameObject("SliderBinding");
		var binding = obj.AddComponent<SliderBinding>();
		obj.transform.SetParent(parent);

		binding.Bind(sliderElement, leftTextFunc, valueFunc);
		return binding;
	}
}