using UnityEngine;
using QGame;
using UnityEngine.UIElements;
using System;

public class PartyTabController : TabController
{
	public override string TabName => "party";
	public override string TabDisplayName => "Party"; 
	
	[SerializeField]
	private VisualTreeAsset _partyUiTemplate;

	private VisualElement _partyRoot;

	protected override void OnInitialize()
	{
		_partyRoot = _partyUiTemplate.Instantiate();
		_partyRoot.style.flexGrow = 1;
		TabView.Add(_partyRoot);
	}
}