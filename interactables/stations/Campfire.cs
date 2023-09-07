using Godot;
using System;

public partial class Campfire : Station
{
	public override void Initialise(Tile tile)
	{
		base.Initialise(tile);
		GetParentTile().LightScene();
	}

	public override void _Ready()
	{
		base._Ready();
	}

	public override void _Process(double delta)
	{

	}

    public override void Interact()
	{
		
	}

    public override void Disengage(Player player)
    {

    }

    public override void Engage(Player player)
    {

    }

	public void OnAnimationFinished(string prevAnimation)
	{
		if (prevAnimation == "Start")
		{
			PlayAnimation("Burn");
		}
	}
}