using Godot;
using System;

public partial class Stall : Station
{
	public override void _Ready()
	{
		base._Ready();
	}

    public override bool Interact()
	{
		return true;
	}

    public override void Disengage()
    {
		base.Disengage();
    }

    public override bool Engage(Player player)
    {
		base.Engage(player);
		return true;
    }

	// GetGlobalTransformWithCanvas().Origin + new Vector2(38, -21.75f)

	public override string GetPlayerAnimation()
	{
		return "idle";
	}
}