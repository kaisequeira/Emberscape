using Godot;
using System;

public partial class Water : Area2D
{
	public void OnWaterEntered(Node2D node)
	{
		if (node is InteractableItem)
		{
			(node as InteractableItem).SetInWater(true);
		}
		else if (node is Hook)
		{
			(node as Hook).TransitionToWade();
		}
	}

	public void OnWaterExited(Node2D node)
	{
		if (node is InteractableItem)
		{
			node.QueueFree();
		}
	}
}
