using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class Day : Node2D
{
	private const int NUM_TILES = 6;
	private const int TILE_FIRST = 0;
	private const int TILE_LAST = 2;
	private const int TILE_MAIN = 1;

	private const int SPAWN_OFFSET_X = 127;
	private const int SPAWN_OFFSET_Y = 120;

	private Timer NightTimer;
	private float maxTime;

	private Camera2D ActiveCamera;
	private CustomSignals CS;
	
	private int currTileOffset = 0;
	private List<Tile> tiles = new List<Tile>();
	private List<InteractableItem> items = new List<InteractableItem>();
	private Player player;

	private float maxCampfireCapacity;
	private float maxFoodCapacity;
	private float campfireWarnPercentage = 0.5f;
	private float foodWarnPercentage = 0.5f;

	private float campfireCapacity;
	private float foodCapacity;

	public override void _Ready()
	{
		RenderingServer.SetDefaultClearColor(Colors.Black);
		NightTimer = GetNode<Timer>("NightTimer");
		ActiveCamera = GetNode<Camera2D>("Camera2D");

		CS = GetNode<CustomSignals>("/root/CustomSignals");
		CS.Connect(CustomSignals.SignalName.SpawnItem, new Callable(this, nameof(SpawnItem)));
		CS.Connect(CustomSignals.SignalName.ReFuelFire, new Callable(this, nameof(FuelCampfire)));
		CS.Connect(CustomSignals.SignalName.ConsumeFood, new Callable(this, nameof(EatFood)));

		TileLoader();
		LoadPlayer();

		maxTime = (float) NightTimer.WaitTime;
		NightTimer.Start();
	}

	public void Initialise(float foodCapacity, float campfireCapacity)
	{
		this.foodCapacity = foodCapacity;
		this.campfireCapacity = campfireCapacity;

		maxCampfireCapacity = campfireCapacity;
		maxFoodCapacity = foodCapacity;
	}

	public Player GetPlayer()
	{
		return player;
	}

    public override void _Process(double delta)
    {
        UpdateActiveCamera(player.GlobalPosition);
		campfireCapacity -= (float) delta;
		foodCapacity -= (float) delta;

		UpdateFoodIndicator();
		UpdateFireIndicator();
		UpdateTimer();
    }

	private void UpdateFoodIndicator()
	{
		if (foodCapacity <= 0)
		{

		}
		else if (foodCapacity < (foodWarnPercentage * maxFoodCapacity))
		{
			player.AddModifier("hunger", 0.3f + 0.7f * (foodCapacity /(foodWarnPercentage * maxFoodCapacity)));
			UI.Get().GetIndicator(UI.IndicatorType.Food).SetAnimSpeed(
				0.3f * (foodCapacity /(foodWarnPercentage * maxFoodCapacity))
			);
		}
		else
		{
			player.RemoveModifier("hunger");
			UI.Get().GetIndicator(UI.IndicatorType.Food).QueueFree();
		}
	}

	private void UpdateFireIndicator()
	{
		if (campfireCapacity <= 0)
		{
			PlayerDied();
		}
		else if (campfireCapacity < (campfireWarnPercentage * maxCampfireCapacity))
		{
			UI.Get().ChangeScreenOpacity(
				1 - campfireCapacity / (campfireWarnPercentage * maxCampfireCapacity)
			);
		}
		else
		{
			UI.Get().ChangeScreenOpacity(0);
		}
	}

	private void UpdateTimer()
	{
		float percentage = 1 - (float) NightTimer.TimeLeft / maxTime;
		UI.Get().SetTimeLabel((float)(Math.Round(percentage / (1.0f / 72.0f)) * (1.0f / 72.0f)));
	}

    private void TileLoader()
	{
		// Initialise directory and loaded array
		string tileDirectory = "res://tiles/";
		bool[] loadedTiles = new bool[NUM_TILES];
		
		loadedTiles[TILE_FIRST] = true;
		loadedTiles[TILE_LAST] = true;

		// Generate tiles list
		for (int i = 0; i < NUM_TILES; i++)
		{
			int tileIndex = i switch
			{
				0 => TILE_FIRST,
				NUM_TILES - 1 => TILE_LAST,
				_ => GetNextTile(loadedTiles)
			};

			loadedTiles[tileIndex] = true;

			PackedScene tileScene = (PackedScene) ResourceLoader.Load(tileDirectory + tileIndex + ".tscn");
			Tile tile = (Tile)tileScene.Instantiate();
			tile.Position = new Vector2(i * Tile.Size.X, 0);
			tile.Initialise(tileIndex);
			tiles.Add(tile);
			AddChild(tile);
		}
	}

	private int GetNextTile(bool[] loadedTiles)
	{
		List<int> falseIndexes = new List<int>();

		for (int i = 0; i < NUM_TILES; i++)
			if (!loadedTiles[i]) falseIndexes.Add(i);

		int randomIndex = new Random().Next(falseIndexes.Count);
		return falseIndexes[randomIndex];
	}

	private void LoadPlayer()
	{
		Tile desiredTile = tiles.Find(tile => tile.GetTileIndex() == TILE_MAIN);

		PackedScene playerScene = (PackedScene) ResourceLoader.Load("res://player/Player.tscn");
		player = (Player)playerScene.Instantiate();
		player.Position = new Vector2(desiredTile.Position.X + SPAWN_OFFSET_X, desiredTile.Position.Y + SPAWN_OFFSET_Y);
		AddChild(player);
	}

	private void UpdateActiveCamera(Vector2 playerPosition)
	{
		int tileOffset = (int)Mathf.Floor(playerPosition.X / Tile.Size.X);
		if (currTileOffset != tileOffset)
		{
			ActiveCamera.GlobalPosition = new Vector2(tileOffset * Tile.Size.X, 0);
			currTileOffset = tileOffset;
		}
	}

	public void SpawnItem(ItemStack itemStack, Vector2 velocity, Vector2 position)
	{
		InteractableItem droppedItem = ((PackedScene) ResourceLoader.Load("res://interactables/item/InteractableItem.tscn")).Instantiate() as InteractableItem;
		items.Add(droppedItem);
		AddChild(droppedItem);
		droppedItem.Initialise(
			itemStack,
			position == Vector2.Inf ? player.GlobalPosition + new Vector2(0, -10) : position,
			velocity == Vector2.Inf ? new Vector2(player.IsFacingRight() ? 30 : -30, -10) : velocity
		);
	}

	public void FuelCampfire(float fuelAmount)
	{
		campfireCapacity += fuelAmount;
		campfireCapacity = Math.Clamp(campfireCapacity, 0, maxCampfireCapacity);
	}

	public void EatFood(float fuelAmount)
	{
		foodCapacity += fuelAmount;
		foodCapacity = Math.Clamp(foodCapacity, 0, maxFoodCapacity);
	}

	public void PlayerDied()
	{
		// Reset all progress
		GD.Print("Game over");
		// TransitionToScreen(game over)
	}

	public void DayEnded()
	{
		// increment day count
		GD.Print("Day ended");
		CS.EmitSignal(CustomSignals.SignalName.DayEnded);
		// TransitionToScreen(next day)
	}

}
