using QGame;
using System;
using System.Collections.Generic;
using System.Linq;
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

	private Dictionary<SkillType, List<ActiveSkill>> _activeSkills = new();

	private ActiveSkill _currentSkill;
	public StopWatchNode CurrentSkillStopwatchNode { get; private set; }
	public string CurrentSkillName => _currentSkill.SkillName;
	private BattleParticipant _currentTarget;

	public string NameAndId => $"{DisplayName} {ParticipantId}";

	public BattleStyle BattleStyle { get; private set; }

	private float _hpHealTrigger = 0.7f;

	private bool _isChoosing;
	private bool _isBattleOver;

	public int CurrentMaxDamage()
	{
		return _activeSkills.ContainsKey(SkillType.Attack) ? 
			_activeSkills[SkillType.Attack].Max(skill => skill.BaseDamageMax) : 0;
	}

	public void Initialize(CharacterSheet characterSheet, BattleParty ownParty, BattleParty otherParty)
	{
		DisplayName = characterSheet.CharacterName;
		CurrentHealth = characterSheet.BaseHealth;
		MaxHealth = characterSheet.BaseHealth;
		Avatar = characterSheet.AvatarImage;
		IsEnemy = characterSheet.IsEnemy;
		BattleStyle = characterSheet.Style;
		_ownParty = ownParty;
		_oppositionParty = otherParty;

		foreach(var skill in characterSheet.Skills)
		{
			if(!_activeSkills.ContainsKey(skill.Type))
				_activeSkills[skill.Type] = new List<ActiveSkill>();
			_activeSkills[skill.Type].Add(skill);
		}
	}

	public void Begin()
	{
		QueueToChoose();
	}

	private void ChooseAction()
	{
		if(BattleStyle == BattleStyle.Heal)
		{
			if (AttemptHeal())
				return;
		}

		AttemptAttack();
	}

	private bool AttemptHeal()
	{
		if (TryBeginHeal())
		{
			BeginCast();
			return true;
		}

		return false;
	}

	private bool TryBeginHeal()
	{
		var skill = _activeSkills.ContainsKey(SkillType.Heal) ?
			_activeSkills[SkillType.Heal].First() : null;

		if (skill == null)
		{
			Log.Battle($"{NameAndId} tried to heal but has no heal skill");
			return false;
		}
		
		var other = _ownParty.GetMostDamaged();
		
		if(other == null || other.HealthAsZeroToOne() > _hpHealTrigger)
		{
			Log.Battle($"{DisplayName} {ParticipantId} has no target to heal");
			return false;
		}

		Log.Battle($"{NameAndId} is healing {other.NameAndId}");
		_currentSkill = skill;
		_currentTarget = other;
		return true;
	}

	private bool AttemptAttack()
	{
		if (TryBeginAttack())
		{
			BeginCast();
			return true;
		}
		else
		{
			StopCasting();
			return false;
		}
	}

	private void BeginCast()
	{
		if (_isBattleOver)
		{
			Log.Battle($"{NameAndId} is trying to cast but the battle is over");
			StopCasting();
			return;
		}

		if(CurrentSkillStopwatchNode == null)
		{
			CurrentSkillStopwatchNode = StopWatch.AddNode("Attack", _currentSkill.CastTime);
			CurrentSkillStopwatchNode.OnTick += OnCastComplete;
		}
		CurrentSkillStopwatchNode.Reset();
		OnSkillStart?.Invoke(this);		
	}

	private void QueueToChoose()
	{
		if(_isChoosing)
			return;

		_isChoosing = true;
		OnNextUpdate += ChooseAction;
	}

	public void StopCasting()
	{
		if(CurrentSkillStopwatchNode != null)
		{
			CurrentSkillStopwatchNode.Reset();
			CurrentSkillStopwatchNode.Pause();
		}
		else
			Log.Battle($"{DisplayName} {ParticipantId} has no skill to stop");
	}

	public void EndBattle()
	{
		_isBattleOver = true;
		StopCasting();
	}

	private void OnCastComplete()
	{
		_currentTarget.TakeDamage(Random.Range(_currentSkill.BaseDamageMin, _currentSkill.BaseDamageMax));
		QueueToChoose();
	}

	public bool TryBeginAttack()
	{
		_currentTarget = _oppositionParty.GetRandomAlive();
		_currentSkill = _activeSkills.ContainsKey(SkillType.Attack) ?
			_activeSkills[SkillType.Attack].First() : null;

		if (_currentTarget == null)
		{
			Log.Battle($"{DisplayName} {ParticipantId} has no attack target");
			return false;
		}
		if(_currentSkill == null){
			Log.Battle($"{DisplayName} {ParticipantId} has no attack skill");
			return false;
		}

		_currentTarget.OnDeath += OnTargetDeath;
		Log.Battle($"{DisplayName} {ParticipantId} is attacking {_currentTarget.DisplayName} {_currentTarget.ParticipantId}");
		return true;
	}

	private void OnTargetDeath(BattleParticipant participant)
	{
		_currentTarget.OnDeath -= OnTargetDeath;
		OnNextUpdate += ChooseAction;
	}

	public void TakeDamage(int damage)
	{
		CurrentHealth = Math.Clamp(CurrentHealth - damage, 0, MaxHealth);
		if (CurrentHealth <= 0)
		{
			StopCasting();
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
