using QGame;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public class BattleCharacterView : QScript
{
	private const string _charNameLabelName = "name-label";
	private const string _avatarName = "avatar-image";
	
	private const string _deathIconName = "death-icon";
	private const string _skillIconImageName = "icon-image";

	private const string _sliderName = "cast-slider"; 
	private const string _hpSliderName = "hp-slider";
	private const string _xpSliderName = "xp-slider";

	private Label _charNameLabel;
	private VisualElement _deathIcon;
	private Image _skillIconImage;

	private Image _avatar;
	private BattleParticipant _participant;

	private VisualElement _skillSlider;
	private VisualElement _hpSlider;
	private VisualElement _xpSlider;

	public string Tracking;

	public void BindToView(TemplateContainer uiDocument)
	{
		_participant = GetComponent<BattleParticipant>();
		_participant.OnSkillStart += OnSkillStart;
		_participant.OnStatUpdate += UpdateValues;
		_participant.OnDeath += HandleDeath;
		Tracking = _participant.NameAndId;
		FindAllElements(uiDocument);
		BindSkillSlider();
		UpdateValues();
	}

	private void BindSkillSlider()
	{
		SliderBindingFactory.CreateSliderBinding(_skillSlider, () => _participant.CurrentSkillName, 
			() => _participant.CurrentSkillStopwatchNode.ElapsedLifetimeAsZeroToOne, transform);

		SliderBindingFactory.CreateSliderBinding(_hpSlider, () => $"HP: {_participant.CurrentHealth} / {_participant.MaxHealth}", 
						() => _participant.CurrentHealth / (float)_participant.MaxHealth, transform);

		SliderBindingFactory.CreateSliderBinding(_xpSlider, () => $"XP: 200 / 1200",
						() => 1, transform);
	}

	private void HandleDeath(BattleParticipant participant)
	{
		Log.Battle($"View handling death of {_participant.DisplayName} {_participant.ParticipantId}");
		_deathIcon.visible = true;
	}

	private void OnSkillStart(BattleParticipant participant)
	{
		_skillIconImage.sprite = participant.CurrentSkillIcon;
	}

	private void FindAllElements(TemplateContainer uiDocument)
	{
		_charNameLabel = uiDocument.Q<Label>(_charNameLabelName);
		_avatar = uiDocument.Q<Image>(_avatarName);
		_skillSlider = uiDocument.Q<VisualElement>(_sliderName);
		_deathIcon = uiDocument.Q<VisualElement>(_deathIconName);
		_skillIconImage = uiDocument.Q<Image>(_skillIconImageName);
		_hpSlider = uiDocument.Q<VisualElement>(_hpSliderName);
		_xpSlider = uiDocument.Q<VisualElement>(_xpSliderName);
	}

	private void UpdateValues()
	{
		UpdateValues(_participant);
	}

	private void UpdateValues(BattleParticipant participant)
	{
		_charNameLabel.text = participant.DisplayName;
		_avatar.sprite = participant.Avatar;
	}

	private string FormatMax(int max)
	{
		return $"/ {max}";
	}
}