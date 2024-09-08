using UnityEngine;

public static class Log
{
	private static string _battlePrefix = "[Battle]";
	public static void Battle(string message)
	{
		Debug.Log($"{_battlePrefix} {message}");
	}
	
	public static void Plain(string message)
	{
		Debug.Log(message);
	}
}


