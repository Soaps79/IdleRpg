using UnityEngine;
using QGame;
using System;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineAnimate))]
public class OreRunner : QScript
{
    public OreInventory OreInventory = new OreInventory();

	[SerializeField]
	private float _upperEnd;
	[SerializeField]
	private float _lowerEnd;
	[SerializeField]
	private int _baseCapacity;

    public Action<OreRunner> OnReachMine;
    public Action<OreRunner> OnReachTown;

	[SerializeField]
	private bool _movingTowardsMine = true;

	private float _lastNormalizedTime;

	private SplineAnimate _animator;
	private Mine _mine;
	private Home _home;

	private void Start()
	{
		_animator = GetComponent<SplineAnimate>();	
	}

	public void Initialize(Mine	mine, Home home)
	{
		_mine = mine;
		_home = home;
	}

	protected override void OnUpdate()
	{
		if (_movingTowardsMine)
		{
			if (_animator.NormalizedTime < _lastNormalizedTime)
			{
				HandleReachMine();
			}
		}
		else
		{
			if (_animator.NormalizedTime > _lastNormalizedTime)
			{
				HandleReachTown();
			}
		}
		_lastNormalizedTime = _animator.NormalizedTime;
	}

	private void HandleReachMine()
	{
		_movingTowardsMine = false;
		var ore = _mine.GetOre(_baseCapacity);
		foreach (var oreAmount in ore)
		{
			OreInventory.AddOre(oreAmount.OreType, oreAmount.Amount);
		}
		OnReachMine?.Invoke(this);
	}

	private void HandleReachTown()
	{
		_movingTowardsMine = true;
		var purge = OreInventory.Purge();
		foreach (var oreAmount in purge)
		{
			_home.IntakeOre(oreAmount.OreType, oreAmount.Amount);
		}
		OnReachMine?.Invoke(this);
	}
}