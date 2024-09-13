using UnityEngine;
using QGame;
using System.Collections.Generic;
using System;

using Random = UnityEngine.Random;

public class QuestManager : QScript
{
    public float CurrentProgress = 0.0f;
    
	public QuestSo ActiveQuest { get; private set; }
    private float _questLength = 0.0f;
	private bool _isProgressing = false;
    private float _timeSinceLastBattle = 0.0f;

    private float _intervalVariance = 0.5f;
    private float _intervalBeforeBoss = 2.0f;

    private List<float> _battlePoints = new List<float>();
    private float _nextBattlePoint = 0.0f;
    private int _currentBattleIndex = 0;
    private bool _nextBattleIsBoss = false;
	private BattleManager _battleManager;
	private CharacterSheet[] _player;

	public void Initialize(BattleManager battleManager, CharacterSheet[] player)
    {
		_battleManager = battleManager;
        _battleManager.OnBattleComplete += CompleteBattle;
        _player = player;
    }

    public void BeginQuest(QuestSo quest)
    {
        ActiveQuest = quest;
        _isProgressing = true;
    
        Log.Quest($"Beginning quest");

        _questLength = ActiveQuest.QuestLength;
        var interval = (ActiveQuest.QuestLength - _intervalBeforeBoss) / ActiveQuest.BattleCount;
        for (int i = 1; i < ActiveQuest.BattleCount; i++)
        {
			_battlePoints.Add(interval * i + Random.Range(-_intervalVariance, _intervalVariance));
		}

        _nextBattlePoint = _battlePoints[0];
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
        _battleManager.BeginBattle(_player, ActiveQuest.EnemyParties[0].Enemies);
        OnBattleBegin?.Invoke();
    }

    public void CompleteBattle()
    {
        _currentBattleIndex++;
		if (_currentBattleIndex >= _battlePoints.Count)
        {
            _nextBattlePoint = _questLength;
			_nextBattleIsBoss = true;
		}
		else
        {
			_nextBattlePoint = _battlePoints[_currentBattleIndex];
		}
		_isProgressing = true;
        OnQuestResume?.Invoke();
        Log.Quest("Resuming quest");
	}

    public float ProgressAsZeroToOne()
    {
		return CurrentProgress / _questLength;
	}

    public Action OnQuestResume;
    public Action OnBattleBegin;

}