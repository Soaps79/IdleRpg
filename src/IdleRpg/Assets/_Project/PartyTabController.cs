using UnityEngine;
using QGame;
using UnityEngine.UIElements;
using System;

public class PartyTabController : TabController
{
	[SerializeField]
	private VisualTreeAsset _partyUiTemplate;

	private VisualElement _partyRoot;

	public override string TabName => "party";

	protected override void OnInitialize()
	{
		_partyRoot = _partyUiTemplate.Instantiate();
		_partyRoot.style.flexGrow = 1;
		TabView.Add(_partyRoot);
	}
}