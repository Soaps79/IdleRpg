
using UnityEngine;
using QGame;
using UnityEngine.UIElements;

public abstract class TabController : QScript
{
	protected VisualElement TabView;
	public abstract string TabName { get; }

	public void SetVisible(bool visible)
	{
		TabView.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
	}

	public void Initialize(VisualElement element)
	{
		TabView = element;
		OnInitialize();
		SetVisible(false);
	}

	protected abstract void OnInitialize();
}