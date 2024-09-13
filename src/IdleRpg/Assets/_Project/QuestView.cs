using UnityEngine;
using QGame;
using UnityEngine.UIElements;
using System;

public class QuestView : QScript
{
    private const string ProgressName = "progress-bkg";
    private const string ImageName = "main-image";
	private const string _enemyPartyContainerName = "enemy-party";

	private VisualElement _progress;
    private Image _image;

    private bool _isProgressing;
	private QuestManager _questManager;

	private UIDocument _questUiDocument;

	private float _maxProgressPercent;

	public void Initialize(QuestManager questManager, UIDocument mainScreenDocument, UIDocument questUiDocument)
    {
		_questManager = questManager;
		_questManager.OnBattleBegin += PauseProgress;
		_questManager.OnQuestResume += ResumeProgress; 
		
		_questUiDocument = questUiDocument;
		_questUiDocument.enabled = true;
		_questUiDocument.rootVisualElement.visible = false;
		_progress = _questUiDocument.rootVisualElement.Q<VisualElement>(ProgressName);
		_image = _questUiDocument.rootVisualElement.Q<Image>(ImageName);
		_image.sprite = _questManager.ActiveQuest.BattleBackground;
		mainScreenDocument.rootVisualElement.Q<VisualElement>(_enemyPartyContainerName).Add(_questUiDocument.rootVisualElement);
    }

	protected override void OnUpdate()
	{
		if (_isProgressing)
		{
			_progress.style.maxWidth = Length.Percent(_maxProgressPercent * _questManager.CurrentProgress * 100);
		}
	}

	private void ResumeProgress()
	{
		_isProgressing = true;
	}

	private void PauseProgress()
	{
		_isProgressing = false;
	}
}