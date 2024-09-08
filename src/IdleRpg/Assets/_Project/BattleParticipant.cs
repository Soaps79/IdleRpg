using QGame;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleParticipant : QScript
{
	public string DisplayName { get; private set; }
	public int CurrentHealth { get; private set; }
	public int MaxHealth { get; private set; }

	public Sprite Avatar { get; private set; }
	public int ParticipantId { get; set; }
	public bool IsEnemy { get; private set; }

	private BattleParty _oppositionParty;
	private BattleParty _ownParty;

	private ActiveSkill _currentSkill;
	public StopWatchNode CurrentSkillStopwatchNode { get; private set; }
	public string CurrentSkillName => _currentSkill.SkillName;
	private BattleParticipant _currentTarget;

	public string NameAndId => $"{DisplayName} {ParticipantId}";

	public int CurrentMaxDamage()
	{
		return _currentSkill.BaseDamageMax;
	}

	public void Initialize(CharacterSheet characterSheet, BattleParty ownParty, BattleParty otherParty)
	{
		DisplayName = characterSheet.CharacterName;
		CurrentHealth = characterSheet.BaseHealth;
		MaxHealth = characterSheet.BaseHealth;
		Avatar = characterSheet.AvatarImage;
		IsEnemy = characterSheet.IsEnemy;
		_currentSkill = characterSheet.StartingSkill;
		_ownParty = ownParty;
		_oppositionParty = otherParty;
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
			Log.Battle($"{DisplayName} {ParticipantId} has no skill to stop");
	}

	private void OnCastComplete()
	{
		_currentTarget.TakeDamage(Random.Range(_currentSkill.BaseDamageMin, _currentSkill.BaseDamageMax));
		BeginAttack();
	}

	public bool GetTarget()
	{
		_currentTarget = _oppositionParty.GetRandomAlive();
		if (_currentTarget == null)
		{
			Log.Battle($"{DisplayName} {ParticipantId} has no target");
			return false;
		}

		_currentTarget.OnDeath += OnTargetDeath;
		Log.Battle($"{DisplayName} {ParticipantId} is attacking {_currentTarget.DisplayName} {_currentTarget.ParticipantId}");
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
			Log.Battle($"{DisplayName} {ParticipantId} has died");
			OnDeath?.Invoke(this);			
		}
		OnStatUpdate?.Invoke(this);
	}

	public float HealthAsZeroToOne()
	{
		return (float)CurrentHealth / MaxHealth;
	}

	public Action<BattleParticipant> OnDeath;
	public Action<BattleParticipant> OnSkillStart;
	public Action<BattleParticipant> OnStatUpdate;
}
