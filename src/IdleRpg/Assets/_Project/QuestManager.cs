using UnityEngine;
using QGame;
using System.Collections.Generic;
using System;

using Random = UnityEngine.Random;

public class QuestManager : QScript
{
    private float CurrentProgress = 0.0f;
    
	public QuestSo ActiveQuest { get; private set; }
    private float _questLength = 0.0f;
	private bool _isProgressing = false;
    private float _timeSinceLastBattle = 0.0f;

    // used in deciding timing between battles
    [SerializeField]
	[Tooltip("Allowed variance from interpolated battle times")]
	private float _intervalVariance;
	[SerializeField]
	[Tooltip("Forced time before boss battle, should be greater than Internal Variance")]
	private float _intervalBeforeBoss;
	[SerializeField]
	[Tooltip("Forced time before first battle, should be greater than Internal Variance")]
	private float _intervalBeforeFirst;

	private List<float> _battlePoints = new List<float>();
    private float _nextBattlePoint = 0.0f;
    private int _currentBattleIndex = 0;
    private bool _nextBattleIsBoss = false;
	public BattleManager BattleManager { get; private set; }
	private CharacterSheet[] _player;

	public void Initialize(BattleManager battleManager, CharacterSheet[] player)
    {
		BattleManager = battleManager;
        BattleManager.OnBattleComplete += CompleteBattle;
        _player = player;
    }

    public void BeginQuest(QuestSo quest)
    {
        ActiveQuest = quest;
        _isProgressing = true;
    
        _questLength = ActiveQuest.QuestLength;
        var interval = (ActiveQuest.QuestLength - _intervalBeforeFirst - _intervalBeforeBoss) / ActiveQuest.BattleCount;
        for (int i = 1; i <= ActiveQuest.BattleCount; i++)
        {
			_battlePoints.Add(_intervalBeforeFirst + interval * i + Random.Range(-_intervalVariance, _intervalVariance));
		}

        _nextBattlePoint = _battlePoints[0];
		Log.Quest($"Beginning quest - point: 0, {_battlePoints[0]} seconds");
	}

	protected override void OnUpdate()
	{
        if (!_isProgressing)
        {
			return;
		}

		CurrentProgress += Time.deltaTime;
        _timeSinceLastBattle += Time.deltaTime;

        if (_timeSinceLastBattle >= _nextBattlePoint)
        {
			Battle();
		}
	}

    private void Battle()
    {
        _isProgressing = false;
        Log.Quest("Switching to Battle");
        if(_nextBattleIsBoss)
        {
			BattleManager.BeginBattle(_player, ActiveQuest.BossParty[0].Enemies);
		}
        else
        {
			BattleManager.BeginBattle(_player, ActiveQuest.EnemyParties[0].Enemies);
		}

		OnBattleBegin?.Invoke();
    }

    public void CompleteBattle()
    {
        _currentBattleIndex++;
		if (_currentBattleIndex >= _battlePoints.Count)
        {
            _nextBattlePoint = _questLength;
			_nextBattleIsBoss = true;
			Log.Quest($"Resuming quest - point: Boss, {_questLength} seconds");
		}
		else
        {
			_nextBattlePoint = _battlePoints[_currentBattleIndex];
			Log.Quest($"Resuming quest - point: {_currentBattleIndex}, {_battlePoints[_currentBattleIndex]} seconds");
		}
		_isProgressing = true;
        OnQuestResume?.Invoke();
        
	}

    public float ProgressAsZeroToOne()
    {
		return CurrentProgress / _questLength;
	}

    public Action OnQuestResume;
    public Action OnBattleBegin;

}