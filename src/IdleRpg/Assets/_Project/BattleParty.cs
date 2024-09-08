using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BattleParty
{
	private readonly List<BattleParticipant> _participants = new();

	public void AddParticipant(BattleParticipant participant)
	{
		_participants.Add(participant);
	}

	public bool IsEveryoneDead()
	{
		return _participants.All(p => p.CurrentHealth <= 0);
	}

	public void Begin()
	{
		foreach (var participant in _participants)
		{
			participant.Begin();
		}
	}

	public void StopAllActions()
	{
		foreach (var participant in _participants)
		{
			participant.StopCasting();
		}
	}

	public BattleParticipant GetHighestDamage()
	{
		if (!_participants.Any())
			return null;

		return _participants.OrderByDescending(p => p.CurrentMaxDamage()).First();
	}

	public BattleParticipant GetMostDamaged()
	{
		if (!_participants.Any())
			return null;

		return _participants.OrderBy(p => p.HealthAsZeroToOne()).First();
	}

	public BattleParticipant GetRandomAlive()
	{
		if (!_participants.Any())
			return null;

		var alive = _participants.Where(p => p.CurrentHealth > 0).ToList();
		return alive.Any() ? alive[Random.Range(0, alive.Count)] : null;
	}
}