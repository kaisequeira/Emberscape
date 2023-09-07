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
	private Camera2D ActiveCamera;
	
	private int dayNum;
	private int currTileOffset = 0;
	private List<Tile> tiles = new List<Tile>();
	private Player player;

	public void Initialise(int dayNum)
	{
		this.dayNum = dayNum;
	}

	public override void _Ready()
	{
		RenderingServer.SetDefaultClearColor(Colors.Black);
		NightTimer = GetNode<Timer>("NightTimer");
		ActiveCamera = GetNode<Camera2D>("Camera2D");
		
		TileLoader();
		LoadPlayer();
	}

	public int GetDayCount()
	{
		return dayNum;
	}

    public override void _Process(double delta)
    {
        UpdateActiveCamera(player.GlobalPosition);
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
			tile.Position = new Vector2(i * TYPES.TILE_SIZE.X, 0);
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
		int tileOffset = (int)Mathf.Floor(playerPosition.X / TYPES.TILE_SIZE.X);
		if (currTileOffset != tileOffset)
		{
			ActiveCamera.GlobalPosition = new Vector2(tileOffset * TYPES.TILE_SIZE.X, 0);
			currTileOffset = tileOffset;
		}
	}

}