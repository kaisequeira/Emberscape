using Godot;
using System;

public abstract partial class Interactable : Node2D
{
	private bool canInteract;

	public abstract void Engage(Player player);
	
	public abstract void Interact();

	public abstract void Disengage(Player player);

	public bool CanInteract()
	{
		return canInteract;
	}

	public void Enable()
	{
		canInteract = true;
	}

	public void Disable()
	{
		canInteract = false;
	}
}
