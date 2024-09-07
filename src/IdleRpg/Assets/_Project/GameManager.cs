using UnityEngine;
using QGame;
using UnityEngine.UIElements;

public class GameManager : QScript
{
    public UIDocument UIDocument;
	public TimedAction AutoAttack;

	public CharacterSheet CharacterSheet;

	public BattleCharacterView _battleCharacterView;

	private void Start()
	{
		//var slider = UIDocument.rootVisualElement.Q<Slider>("my_slider");
		//AutoAttack.BindToSlider(slider);

		//_battleCharacterView.BindToView(CharacterSheet, UIDocument);
	}

}