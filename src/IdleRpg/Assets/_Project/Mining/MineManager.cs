using UnityEngine;
using QGame;
using UnityEngine.Splines;
using System;

[Serializable]
public class MinePath
{
    public SplineContainer Path;
    public Mine Mine;
}

public class MineManager : QScript
{
    [SerializeField]
    private Home _home;

    [SerializeField] 
    private MinePath[] _minePaths;

    [SerializeField]
    private MineRunner _oreRunnerPrefab;

	private void Start()
	{
        AddRunnerOnPath(_minePaths[0]);
	}

	private void AddRunnerOnPath(MinePath minePath)
    {
		var runner = Instantiate(_oreRunnerPrefab, minePath.Mine.transform);
        var animator = runner.GetComponent<SplineAnimate>();
        animator.Container = minePath.Path;
        animator.enabled = true;

        runner.Initialize(minePath.Mine, _home);
	}
}