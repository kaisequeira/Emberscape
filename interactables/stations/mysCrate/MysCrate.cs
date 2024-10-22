using Godot;
using Godot.Collections;
using System;

public partial class MysCrate : Station
{
	[Export]
	private Array<string> outputItems = new Array<string>();

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
		Disable();
    }

    public override bool Engage(Player player)
    {
		base.Engage(player);
		
		Item item = Activator.CreateInstance(Type.GetType(outputItems[GD.RandRange(0, outputItems.Count - 1)])) as Item;
		ItemStack randomStack = new ItemStack(item, item.GetMaxStack());
		CS.EmitSignal(CustomSignals.SignalName.SpawnItem, randomStack, new Vector2(0, -30), GlobalPosition + new Vector2(0, -10));
		
		return false;
    }

	public override string GetPlayerAnimation()
	{
		return "idle";
	}
}