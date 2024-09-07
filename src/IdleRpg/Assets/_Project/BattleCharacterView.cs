using QGame;
using UnityEngine.UIElements;

public class BattleCharacterView : QScript
{
	private const string _charNameLabelName = "char_name";
	private const string _currentHpLabelName = "hp-current";
	private const string _maxHpLabelName = "hp-total";
	private const string _currentXpLabelName = "xp-current";
	private const string _maxXpLabelName = "xp-next-level";
	private const string _avatarName = "char_portrait";

	private Label _currentHpLabel;
	private Label _maxHpLabel;
	private Label _currentXpLabel;
	private Label _maxXpLabel;
	private Label _charNameLabel;

	private Image _avatar;
	private CharacterSheet _characterSheet;

	public void BindToView(CharacterSheet characterSheet, UIDocument uiDocument)
	{
		_characterSheet = characterSheet;
		FindAllElements(uiDocument);
		SetStartingValues();		
	}

	private void FindAllElements(UIDocument uiDocument)
	{
		_charNameLabel = uiDocument.rootVisualElement.Q<Label>(_charNameLabelName);
		_currentHpLabel = uiDocument.rootVisualElement.Q<Label>(_currentHpLabelName);
		_maxHpLabel = uiDocument.rootVisualElement.Q<Label>(_maxHpLabelName);
		_currentXpLabel = uiDocument.rootVisualElement.Q<Label>(_currentXpLabelName);
		_maxXpLabel = uiDocument.rootVisualElement.Q<Label>(_maxXpLabelName);
		_avatar = uiDocument.rootVisualElement.Q<Image>(_avatarName);
	}

	private void SetStartingValues()
	{
		_charNameLabel.text = _characterSheet.CharacterName;
		_currentHpLabel.text = _characterSheet.BaseHealth.ToString();
		_maxHpLabel.text = FormatMax(_characterSheet.BaseHealth);
		_currentXpLabel.text = "200";
		_maxXpLabel.text = FormatMax(2400);
		_avatar.sprite = _characterSheet.AvatarImage;
	}

	private string FormatMax(int max)
	{
		return $"/ {max}";
	}
}