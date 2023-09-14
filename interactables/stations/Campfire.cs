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

    public override void Interact()
	{
		
	}

    protected override void Disengaged(Player player)
    {

    }

    protected override bool Engaged(Player player)
    {
		return true;
    }

	public void OnAnimationFinished(string prevAnimation)
	{
		if (prevAnimation == "Start")
		{
			PlayAnimation("Burn");
		}
	}
}