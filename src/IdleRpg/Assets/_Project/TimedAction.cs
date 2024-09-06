using UnityEngine;
using QGame;
using UnityEngine.UIElements;

public class TimedAction : QScript
{
	[SerializeField]
	private float Duration;
	[SerializeField]
	private string Name;

	public void BindToSlider(Slider slider)
	{
		slider.label = Name;

		var node = StopWatch.AddNode("test", Duration);
		OnEveryUpdate += () =>
		{
			slider.value = node.ElapsedLifetimeAsZeroToOne;
			Debug.Log(node.ElapsedLifetimeAsZeroToOne);
		};
	}
}