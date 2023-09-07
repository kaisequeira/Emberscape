using Godot;
using System;
using System.Collections.Generic;

public partial class Tile : Node2D
{
	private AnimationPlayer AnimationPlayer;
	private int tileNum;
	private List<Station> stations = new List<Station>();

	public void Initialise(int tileNum)
	{
		this.tileNum = tileNum;
	}

	public override void _Ready()
	{
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		AddStations();
	}

	public int GetTileIndex()
	{
		return tileNum;
	}

	public void AddStations()
	{
		foreach (Node2D station in GetNode<Node2D>("Stations").GetChildren())
			stations.Add((Station) station);

		foreach(Station station in stations)
			station.Initialise(this);
	}

	public void LightScene()
	{
		AnimationPlayer.Play("Start");

		foreach (Station station in stations)
		{
			station.Light();

			if (!(station is Torch))
			{
				station.Enable();
			}
			else
			{
				station.Disable();
			}
		}
	}

}