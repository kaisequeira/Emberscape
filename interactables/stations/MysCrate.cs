using Godot;
using System;

public partial class MysCrate : Station
{
	public override void _Ready()
	{
		base._Ready();
	}

    public override void Interact()
	{
		
	}

    protected override void Disengaged(Player player)
    {
		InvManager.Get().GetInventory(Inventory.Types.Mystery).Disable();
    }

    protected override bool Engaged(Player player)
    {
		InvManager.Get().GetInventory(Inventory.Types.Mystery).Enable();
		return true;
    }
}