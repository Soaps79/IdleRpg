using UnityEngine;
using QGame;
using UnityEngine.UIElements;
using System;

public class QuestView : QScript
{
    private const string ProgressName = "progress-bkg";
    private const string ImageName = "main-image";

	private const string ProgressSliderName = "progress-slider";

	private const string MainBkgName = "main-bkg";
	
	private const string TopBoxName = "top-box";
	private const string BottomBoxName = "bottom-box";

	private VisualElement _progress;
    private Image _image;

    private bool _isProgressing;
	private QuestManager _questManager;

	[SerializeField]
	private VisualTreeAsset _questUiTemplate;

	[SerializeField]
	private VisualTreeAsset _questProgressTemplate;

	[SerializeField]
	private BattleView _battleViewPrefab;

	private float _maxProgressPercent;
	private VisualElement _tabRoot;
	private VisualElement _progressRoot;
	private VisualElement _questUiRoot;

	private Slider _progressSlider;

	public void Initialize(QuestManager questManager, VisualElement root)
    {
		_questManager = questManager;
		_questManager.OnBattleBegin += HandleBattleBegin;
		_questManager.OnQuestResume += HandleQuestResume;

		_tabRoot = root;

		//_tabRoot.Add(new Label() { text = "JSKNFWKNFKNSEKF"});
		_questUiRoot = _questUiTemplate.Instantiate();
		_questUiRoot.style.flexGrow = 1;
		_tabRoot.Add(_questUiRoot);

		CreateProgressUi();		
    }

	private void CreateProgressUi()
	{
		_progressRoot = _questProgressTemplate.Instantiate();
		_progressRoot.style.flexGrow = 1;

		_progressSlider = _progressRoot.Q<Slider>(ProgressSliderName);
		_progress = _progressRoot.Q<VisualElement>(ProgressName);
		_image = _progressRoot.Q<Image>(ImageName);
		_questUiRoot.Q<VisualElement>(TopBoxName).Add(_progressRoot);

		_isProgressing = true;
	}

	protected override void OnUpdate()
	{
		if (_isProgressing)
		{
			_progressSlider.value = _questManager.ProgressAsZeroToOne();
			//_progress.style.maxWidth = Length.Percent(_maxProgressPercent * _questManager.CurrentProgress * 100);
		}
	}

	private void HandleBattleBegin()
	{
		var obj = Instantiate(_battleViewPrefab, transform);
		obj.BeginBattle(_questManager.BattleManager, _tabRoot);
		_isProgressing = false;

		//_progressRoot.visible = false;
		_progressRoot.style.display = DisplayStyle.None;
	}

	private void HandleQuestResume()
	{
		_isProgressing = true;

		//_progressRoot.visible = true;
		_progressRoot.style.display = DisplayStyle.Flex;
	}	
}