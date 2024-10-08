using UnityEngine;
using QGame;
using System;

public class CraftingContainer : QScript
{
	private bool _isCrafting;
	public ProductRecipe CurrentRecipe;

	private float _elapsedTime;
	private float _craftTime;

	public Action<ProductRecipe> OnCraftComplete;

	public void BeginCrafting(ProductRecipe recipe)
	{
		CurrentRecipe = recipe;
		_craftTime = recipe.Duration;
		_isCrafting = true;
	}

	protected override void OnUpdate()
	{
		if (!_isCrafting)
			return;

		_elapsedTime += Time.deltaTime;

		do
		{
			if (_elapsedTime >= _craftTime)
			{
				_elapsedTime -= _craftTime;
				OnCraftComplete?.Invoke(CurrentRecipe);
			}
		} while (_elapsedTime >= _craftTime);
	}

	public float CurrentCraftProgress
	{
		get
		{
			if (!_isCrafting)
				return 0;

			return _elapsedTime / _craftTime;
		}
	}
}


// from VoidSim, for reference
/// <summary>
/// This object manages the queueing and crafting of Recipes. It was written alongside
/// PlayerCraftingViewModel, which binds it to a player-facing UI. Any other actor can drive
/// the Container using a similar interface:
/// 
/// QueueCrafting(Recipe recipe) - queues a build, returns its ID
/// public Action<Recipe> OnCraftingComplete - consume the result
/// 
/// CancelCrafting(int recipeId)
/// public Action<Recipe> OnCraftingCancelled - refunds goods
/// 
/// </summary>
//public class CraftingContainer : QScript
//{
//	// for use internally and for UI callbacks
//	public class QueuedRecipe
//	{
//		public int ID;
//		public Recipe Recipe;
//	}

//	public CraftingContainerInfo Info;
//	public IWorldClock WorldClock;

//	private readonly List<QueuedRecipe> _recipeQueue = new List<QueuedRecipe>();
//	private QueuedRecipe _currentlyCrafting;
//	private const string STOPWATCH_NAME = "Crafting";

//	// best external usage
//	public Action<Recipe> OnCraftingComplete;
//	public Action<Recipe> OnCraftingCancelled;

//	// could probably be ignored by an outside actor, aimed at internal UI representation
//	// also encapsulates Recipe ID system
//	public Action<Recipe, int> OnCraftingBegin;
//	public Action<Recipe, int> OnCraftingQueued;
//	public Action<Recipe, int> OnCraftingCompleteUI;
//	private string _lastIdName;

//	public float CurrentQueueCount { get { return _recipeQueue.Count; } }
//	public float CurrentCraftElapsedAsZeroToOne
//	{
//		get
//		{
//			var returnVal = StopWatch.IsRunning()
//			? StopWatch[STOPWATCH_NAME].ElapsedLifetimeAsZeroToOne : 0f;
//			return returnVal;
//		}
//	}

//	public float CurrentEfficiency
//	{
//		get { return StopWatch.TimeModifier; }
//		set { StopWatch.TimeModifier = value; }
//	}

//	void Awake()
//	{
//		_lastIdName = "crafting_container_" + name;
//	}

//	// This, CancelCrafting() and the Recipe callbacks are the typical in-game usage
//	public int QueueCrafting(Recipe recipe)
//	{
//		var queued = new QueuedRecipe { ID = Locator.LastId.GetNext(_lastIdName), Recipe = recipe };
//		_recipeQueue.Add(queued);

//		if (OnCraftingQueued != null)
//			OnCraftingQueued(queued.Recipe, queued.ID);

//		CheckForBeginCrafting();
//		return queued.ID;
//	}

//	private void CheckForBeginCrafting()
//	{
//		// If nothing is currently being crafted and there is something in the queue, start it.
//		// Doing it next cycle to give stopwatch a chance to complete, should not be made not necessary
//		if (_currentlyCrafting == null && _recipeQueue.Any())
//		{
//			var first = _recipeQueue.First();
//			OnNextUpdate += () => BeginCrafting(first);
//			_recipeQueue.RemoveAt(0);
//		}
//	}

//	private void BeginCrafting(QueuedRecipe queuedRecipe)
//	{
//		var recipe = queuedRecipe.Recipe;

//		// will most likely not make it here in the first place, but jic
//		if (recipe.Container.Name != Info.Name)
//			throw new UnityException(string.Format("{0} was given a recipe for {1}", Info.Name, recipe.Container.Name));

//		// may replace with WorldTime when it is a more flexible type
//		var seconds = Locator.WorldClock.GetSeconds(recipe.TimeLength);

//		StopWatch.AddNode(STOPWATCH_NAME, seconds, true).OnTick = CompleteCraft;
//		_currentlyCrafting = queuedRecipe;
//		if (OnCraftingBegin != null)
//			OnCraftingBegin(queuedRecipe.Recipe, queuedRecipe.ID);
//	}

//	private void CompleteCraft()
//	{
//		// tell the observers it is complete, start the next if there is one
//		if (OnCraftingComplete != null)
//			OnCraftingComplete(_currentlyCrafting.Recipe);

//		if (OnCraftingCompleteUI != null)
//			OnCraftingCompleteUI(_currentlyCrafting.Recipe, _currentlyCrafting.ID);

//		_currentlyCrafting = null;
//		CheckForBeginCrafting();
//	}

//	public void CancelCrafting(int recipeId)
//	{
//		Recipe recipe = null;
//		if (_currentlyCrafting == null)
//		{
//			CheckForBeginCrafting();
//			return;
//		}

//		// check whether to cancel current build or one from the queue
//		if (_currentlyCrafting.ID == recipeId)
//		{
//			recipe = _currentlyCrafting.Recipe;
//			CancelCurrentCraft();
//		}
//		else
//		{
//			var queuedRecipe = _recipeQueue.FirstOrDefault(i => i.ID == recipeId);
//			if (queuedRecipe != null)
//				recipe = queuedRecipe.Recipe;

//			_recipeQueue.RemoveAll(i => i.ID == recipeId);
//		}

//		// only broadcast if a build was actually cancelled
//		if (recipe != null && OnCraftingCancelled != null)
//			OnCraftingCancelled(recipe);

//		CheckForBeginCrafting();
//	}

//	private void CancelCurrentCraft()
//	{
//		_currentlyCrafting = null;
//		StopWatch[STOPWATCH_NAME].Reset(0);
//		StopWatch[STOPWATCH_NAME].Pause();
//	}

//	public int ResumeCrafting(Recipe recipe, float remaining)
//	{
//		var seconds = Locator.WorldClock.GetSeconds(recipe.TimeLength);
//		var node = StopWatch.AddNode(STOPWATCH_NAME, seconds, true);
//		node.OnTick = CompleteCraft;
//		node.UpdateElapsed(remaining * seconds);
//		_currentlyCrafting = new QueuedRecipe { ID = Locator.LastId.GetNext(_lastIdName), Recipe = recipe };
//		if (OnCraftingBegin != null)
//			OnCraftingBegin(_currentlyCrafting.Recipe, _currentlyCrafting.ID);

//		return _currentlyCrafting.ID;
//	}
//}