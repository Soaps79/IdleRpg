using UnityEngine;
using QGame;
using System;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineAnimate))]
public class MineRunner : QScript
{
    public ProductInventory OreInventory = new ProductInventory();

	[SerializeField]
	private float _upperEnd;
	[SerializeField]
	private float _lowerEnd;
	[SerializeField]
	private int _baseCapacity;

    public Action<MineRunner> OnReachMine;
    public Action<MineRunner> OnReachTown;

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
		var ore = _mine.GetProducts(_baseCapacity);
		foreach (var oreAmount in ore)
		{
			OreInventory.AddProduct(oreAmount);
		}
		OnReachMine?.Invoke(this);
	}

	private void HandleReachTown()
	{
		_movingTowardsMine = true;
		var purge = OreInventory.Purge();
		foreach (var oreAmount in purge)
		{
			_home.Inventory.AddProduct(oreAmount);
		}
		OnReachMine?.Invoke(this);
	}
}