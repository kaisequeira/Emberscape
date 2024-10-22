using Godot;
using System;

public abstract partial class Interactable : CharacterBody2D
{
	protected Sprite2D sprite2D;
	private CollisionShape2D collisionShape2D;

	protected Player player;
	protected Inventory playerInv;

	protected CustomSignals CS;
	
	private bool canInteract = false;

	public abstract string GetPlayerAnimation();

	public abstract bool Interact();
	
	public virtual bool Engage(Player player)
	{
		HighlightOff();
		playerInv.SelectSlot(Inventory.INVALID_SLOT);
		this.player = player;
		return true;
	}

	public virtual void Disengage()
	{
		playerInv.SelectSlot(player.GetSelected());
	}

	public override void _Ready()
	{
		playerInv = UI.Get().GetInventory(Inventory.Types.Player);
		CS = GetNode<CustomSignals>("/root/CustomSignals");
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
