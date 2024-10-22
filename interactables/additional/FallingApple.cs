using Godot;
using System;
using System.Diagnostics;

public partial class FallingApple : Area2D
{
	private static readonly int NUMSTAGES = 5;

	private int stage = 0;
	private Vector2 spawnPosition;

	private CustomSignals CS;
	private AppleTree appleTree;

	public override void _Ready()
	{
		CS = GetNode<CustomSignals>("/root/CustomSignals");
		spawnPosition = GlobalPosition;
	}

	public override void _Process(double delta)
	{	
		if (stage == NUMSTAGES)
		{
			CS.EmitSignal("SpawnItem", new ItemStack(new Apple(), 1), Vector2.Zero, GlobalPosition + new Vector2(0, 5.5f));
			Disable();
		}

		GlobalPosition = spawnPosition + new Vector2(0, stage);
	}

	public void Enable(AppleTree appleTree)
	{
		this.appleTree = appleTree;
		GlobalPosition = spawnPosition;
		Visible = true;
	}

	private void Disable()
	{
		Visible = false;
		stage = 0;
	}

	public void OnInput(Node viewport, InputEvent @event, int shape_idx)
	{
		if (Visible && appleTree.isPickable())
		{
			if (@event is InputEventMouseButton mouseEvent
				&& mouseEvent.ButtonIndex == MouseButton.Left
				&& mouseEvent.Pressed)
				stage++;
		}
	}

	public void OnMouseEntered()
	{
		Debug.Print("Entered area");
	}
}
