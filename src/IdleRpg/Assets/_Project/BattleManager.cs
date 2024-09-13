using QGame;
using System;
using System.Collections.Generic;
using UnityEngine;

// Currently manages both Battle and its UI, should split out a View

public class BattleManager : QScript
{
    public GameObject CharacterViewPrefab;
    
	private int _lastId = 0;

    public BattleParty PlayerParty { get; private set; } = new();
    public BattleParty EnemyParty { get; private set; } = new();

	private List<GameObject> _participants = new List<GameObject>();

	public void BeginBattle(CharacterSheet[] player, CharacterSheet[] enemy)
    {
		foreach (var character in player)
        {
            var obj = Instantiate(CharacterViewPrefab, transform);
			_participants.Add(obj);
                        
            var partipant = obj.GetComponent<BattleParticipant>();
            partipant.Initialize(character, PlayerParty, EnemyParty);
            _lastId++;
            partipant.ParticipantId = _lastId;
			partipant.OnDeath += HandleParticipantDeath;

            PlayerParty.AddParticipant(partipant);
        }

		foreach (var character in enemy)
		{
			var obj = Instantiate(CharacterViewPrefab, transform);
			_participants.Add(obj);

			var partipant = obj.GetComponent<BattleParticipant>();
			partipant.Initialize(character, EnemyParty, PlayerParty);
			_lastId++;
			partipant.ParticipantId = _lastId;
			partipant.OnDeath += HandleParticipantDeath;

            EnemyParty.AddParticipant(partipant);
		}

		PlayerParty.Begin();
        EnemyParty.Begin();
    }

	private void HandleParticipantDeath(BattleParticipant participant)
	{
		if(PlayerParty.IsEveryoneDead())
        {
			OnDefeat?.Invoke();
			BeginCompleteBattle();
		}
		else if(EnemyParty.IsEveryoneDead())
        {
			OnVictory?.Invoke();
			BeginCompleteBattle();
		}
	}

	private void BeginCompleteBattle()
	{
		StopAllPartyActions();
		StopWatch.AddNode("done", 2.0f, true).OnTick += () =>
		{
			CompleteBattle();
		};
	}

	private void CompleteBattle()
	{
		foreach (var participant in _participants)
		{
			Destroy(participant);
		}

		_participants.Clear();

		PlayerParty = new BattleParty();
		EnemyParty = new BattleParty();

		OnBattleComplete?.Invoke();
	}

	private void StopAllPartyActions()
	{
		PlayerParty.StopAllActions();
		EnemyParty.StopAllActions();
	}

	public Action OnBattleResult;
	public Action OnBattleComplete;
	public Action OnVictory;
	public Action OnDefeat;
}