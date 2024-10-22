using Godot;
using System;

public partial class CraftingContainer : VBoxContainer
{
	[Export]
	private string buttonPath;

    public override void _EnterTree()
	{
		foreach (CraftingPanel panel in GetChildren())
			panel.SetButtonPath(buttonPath);
	}

	public override void _Process(double delta)
	{

	}
}
