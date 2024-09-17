using UnityEngine;
using QGame;
using UnityEngine.UIElements;
using System;

public class PartyTabController : QScript
{
	[SerializeField]
	private VisualTreeAsset _partyUiTemplate;

	private VisualElement _tabView;
	private VisualElement _partyRoot;

	public void SetVisible(bool visible)
	{
		_tabView.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
	}

	public void Initialize(VisualElement element)
	{
		_tabView = element;
		CreatePartyView();
		SetVisible(false);
	}

	private void CreatePartyView()
	{
		_partyRoot = _partyUiTemplate.Instantiate();
		_partyRoot.style.flexGrow = 1;
		_tabView.Add(_partyRoot);
	}
}