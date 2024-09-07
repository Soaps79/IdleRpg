using QGame;
using System;
using UnityEngine.UIElements;

public class BattleCharacterView : QScript
{
	private const string _charNameLabelName = "char_name";
	private const string _currentHpLabelName = "hp-current";
	private const string _maxHpLabelName = "hp-total";
	private const string _currentXpLabelName = "xp-current";
	private const string _maxXpLabelName = "xp-next-level";
	private const string _avatarName = "char-portrait";
	private const string _sliderName = "skill-slider";

	private Label _currentHpLabel;
	private Label _maxHpLabel;
	private Label _currentXpLabel;
	private Label _maxXpLabel;
	private Label _charNameLabel;

	private Image _avatar;
	private CharacterSheet _characterSheet;
	private Slider _skillSlider;

	public void BindToView(CharacterSheet characterSheet, TemplateContainer uiDocument)
	{
		_characterSheet = characterSheet;
		FindAllElements(uiDocument);
		SetStartingValues();
		BindAttack();
	}

	private void BindAttack()
	{
		_skillSlider.label = _characterSheet.StartingSkill.SkillName;
		var node = StopWatch.AddNode("Attack", _characterSheet.StartingSkill.CastTime);
		OnEveryUpdate += () =>
		{
			_skillSlider.value = node.ElapsedLifetimeAsZeroToOne;
		};
	}

	private void FindAllElements(TemplateContainer uiDocument)
	{
		_charNameLabel = uiDocument.Q<Label>(_charNameLabelName);
		_currentHpLabel = uiDocument.Q<Label>(_currentHpLabelName);
		_maxHpLabel = uiDocument.Q<Label>(_maxHpLabelName);
		_currentXpLabel = uiDocument.Q<Label>(_currentXpLabelName);
		_maxXpLabel = uiDocument.Q<Label>(_maxXpLabelName);
		_avatar = uiDocument.Q<Image>(_avatarName);
		_skillSlider = uiDocument.Q<Slider>(_sliderName);
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