using QGame;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleParticipant : QScript
{
	public string DisplayName;
	public int CurrentHealth;
	public int MaxHealth;
	public BattleManager Manager;
	public Sprite Avatar;
	public int ParticipantId;
	public bool IsEnemy;

	private ActiveSkill _currentSkill;
	public StopWatchNode CurrentSkillStopwatchNode;
	public string CurrentSkillName => _currentSkill.SkillName;
	private BattleParticipant _currentTarget;

	public string NameAndId => $"{DisplayName} {ParticipantId}";

	public void Initialize(CharacterSheet characterSheet, BattleManager manager)
	{
		DisplayName = characterSheet.CharacterName;
		CurrentHealth = characterSheet.BaseHealth;
		MaxHealth = characterSheet.BaseHealth;
		Avatar = characterSheet.AvatarImage;
		IsEnemy = characterSheet.IsEnemy;
		_currentSkill = characterSheet.StartingSkill;
		Manager = manager;	
	}

	public void Begin()
	{
		AttemptAttack();
	}

	private void AttemptAttack()
	{
		if (GetTarget())
		{
			BeginAttack();
		}
		else
		{
			StopAttacking();
		}
	}

	private void BeginAttack()
	{
		if(CurrentSkillStopwatchNode == null)
		{
			CurrentSkillStopwatchNode = StopWatch.AddNode("Attack", _currentSkill.CastTime);
			CurrentSkillStopwatchNode.OnTick += OnCastComplete;
		}
		CurrentSkillStopwatchNode.Reset();
		OnSkillStart?.Invoke(this);		
	}

	private void StopAttacking()
	{
		if(CurrentSkillStopwatchNode != null)
		{
			CurrentSkillStopwatchNode.Reset();
			CurrentSkillStopwatchNode.Pause();
		}
		else
			Debug.Log($"{DisplayName} {ParticipantId} has no skill to stop");
	}

	private void OnCastComplete()
	{
		_currentTarget.TakeDamage(Random.Range(_currentSkill.BaseDamageMin, _currentSkill.BaseDamageMax));
		BeginAttack();
	}

	public bool GetTarget()
	{
		_currentTarget = Manager.GetNextTarget(this);
		if (_currentTarget == null)
		{
			Debug.Log($"{DisplayName} {ParticipantId} has no target");
			return false;
		}

		_currentTarget.OnDeath += OnTargetDeath;
		Debug.Log($"{DisplayName} {ParticipantId} is attacking {_currentTarget.DisplayName} {_currentTarget.ParticipantId}");
		return true;
	}

	private void OnTargetDeath(BattleParticipant participant)
	{
		_currentTarget.OnDeath -= OnTargetDeath;
		OnNextUpdate += AttemptAttack;
	}

	public void TakeDamage(int damage)
	{
		CurrentHealth -= damage;
		if (CurrentHealth <= 0)
		{
			StopAttacking();
			Debug.Log($"{DisplayName} {ParticipantId} has died");
			OnDeath?.Invoke(this);			
		}
		OnStatUpdate?.Invoke(this);
	}

	public Action<BattleParticipant> OnDeath;
	public Action<BattleParticipant> OnSkillStart;
	public Action<BattleParticipant> OnStatUpdate;
}
