using Godot;
using System;

public partial class Logpile : Station
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
		UI.Get().GetInventory(Inventory.Types.Logpile).Disable();
    }

    public override bool Engage(Player player)
    {
		base.Engage(player);
		UI.Get().GetInventory(Inventory.Types.Logpile).Enable();
		return true;
    }

	public override string GetPlayerAnimation()
	{
		return "idle";
	}
}