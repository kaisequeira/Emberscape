using Godot;
using System;

public partial class Stump : Station
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
		UI.Get().GetStumpCrafting().Visible = false;
    }

    public override bool Engage(Player player)
    {
		base.Engage(player);
		UI.Get().GetStumpCrafting().Visible = true;
		return true;
    }

	public override string GetPlayerAnimation()
	{
		return "idle";
	}
}