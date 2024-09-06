using QGame;
using UnityEngine.UIElements;

public class BattleCharacterView : QScript
{
	private Label _hpLabel;

	public void BindToView(CharacterSheet characterSheet, UIDocument uiDocument)
	{
		var name = uiDocument.rootVisualElement.Q<Label>("char_name");
		name.text = characterSheet.CharacterName;

		_hpLabel = uiDocument.rootVisualElement.Q<Label>("hp-value");
		_hpLabel.text = characterSheet.BaseHealth.ToString();

		var avatar = uiDocument.rootVisualElement.Q<Image>("char_portrait");
		avatar.sprite = characterSheet.AvatarImage;
	}
}