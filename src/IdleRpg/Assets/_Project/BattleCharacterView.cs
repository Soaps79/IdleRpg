using QGame;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleCharacterView : QScript
{
	private const string _charNameLabelName = "name-label";
	private const string _currentHpLabelName = "hp-current";
	private const string _maxHpLabelName = "hp-max";
	private const string _currentXpLabelName = "xp-current";
	private const string _maxXpLabelName = "xp-max";
	private const string _avatarName = "avatar-image";
	private const string _sliderName = "cast-slider";
	private const string _deathIconName = "death-icon";

	private Label _currentHpLabel;
	private Label _maxHpLabel;
	private Label _currentXpLabel;
	private Label _maxXpLabel;
	private Label _charNameLabel;
	private VisualElement _deathIcon;

	private Image _avatar;
	private BattleParticipant _participant;
	private Slider _skillSlider;

	public string Tracking;

	public void BindToView(TemplateContainer uiDocument)
	{
		_participant = GetComponent<BattleParticipant>();
		_participant.OnSkillStart += OnSkillStart;
		_participant.OnStatUpdate += UpdateValues;
		_participant.OnDeath += HandleDeath;
		Tracking = _participant.NameAndId;
		FindAllElements(uiDocument);
		UpdateValues();
	}

	private void HandleDeath(BattleParticipant participant)
	{
		Debug.Log($"View handling death of {_participant.DisplayName} {_participant.ParticipantId}");
		OnEveryUpdate -= UpdateSkillSlider;
		_skillSlider.value = 0;
		_deathIcon.visible = true;
	}

	private void OnSkillStart(BattleParticipant participant)
	{
		_skillSlider.label = participant.CurrentSkillName;
		OnEveryUpdate += UpdateSkillSlider;
	}

	private void UpdateSkillSlider()
	{
		if(_participant.CurrentSkillStopwatchNode == null)
		{
			Debug.Log($"No skill node found for {_participant.DisplayName} {_participant.ParticipantId}");
		}
		_skillSlider.value = _participant.CurrentSkillStopwatchNode.ElapsedLifetimeAsZeroToOne;
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
		_deathIcon = uiDocument.Q<VisualElement>(_deathIconName);
	}

	private void UpdateValues()
	{
		UpdateValues(_participant);
	}

	private void UpdateValues(BattleParticipant participant)
	{
		_charNameLabel.text = participant.DisplayName;
		_currentHpLabel.text = participant.CurrentHealth.ToString();
		_maxHpLabel.text = FormatMax(participant.MaxHealth);
		_currentXpLabel.text = "200";
		_maxXpLabel.text = FormatMax(2400);
		_avatar.sprite = participant.Avatar;
	}

	private string FormatMax(int max)
	{
		return $"/ {max}";
	}
}