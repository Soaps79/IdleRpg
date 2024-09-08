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

	public static void Error(string message)
	{
		Debug.LogError(message);
	}

	public static void Warning(string message)
	{
		Debug.LogWarning(message);
	}

	public static void Exception(System.Exception ex)
	{
		Debug.LogException(ex);
	}
}


