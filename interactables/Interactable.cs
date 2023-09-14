using Godot;
using System;

public abstract partial class Interactable : CharacterBody2D
{
	protected Sprite2D sprite2D;
	private CollisionShape2D collisionShape2D;
	private Inventory playerInv;
	private bool canInteract = false;

	public abstract void Interact();
	
	public bool Engage(Player player)
	{
		if (Engaged(player))
		{
			HighlightOff();
			playerInv.SelectSlot(Inventory.INVALID_SLOT);
			return true;
		}
		return false;
	}

	protected abstract bool Engaged(Player player);

	public void Disengage(Player player)
	{
		Disengaged(player);
		playerInv.SelectSlot(player.GetSelected());
		HighlightOn();
	}

	protected abstract void Disengaged(Player player);

	public override void _Ready()
	{
		playerInv = InvManager.Get().GetInventory(Inventory.Types.Player);
		sprite2D = GetNode<Sprite2D>("Sprite2D");
		collisionShape2D = GetNode<Area2D>("Area2D")
						  .GetNode<CollisionShape2D>("CollisionShape2D");
	}

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

	public void HighlightOn()
	{
		sprite2D.Material = ResourceLoader.Load("res://art/interactables/interactableOutline.tres") as Material;
	}

	public void HighlightOff()
	{
		sprite2D.Material = default(Material);
	}

	public void AddToPlayer(Node2D body)
	{
		(body as Player).AddInteractable(this);
	}

	public void RemoveFromPlayer(Node2D body)
	{
		(body as Player).RemoveInteractable(this);
	}

	public Vector2 GetPosition()
	{
		return collisionShape2D.GlobalPosition;
	}
}
