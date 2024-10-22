using Godot;
using System;
using System.Collections.Generic;

public partial class InteractableItem : Interactable
{
	private List<InteractableItem> itemsInRange = new List<InteractableItem>();
	private ItemStack itemStack;
	private bool inWater;

	public override void _Ready()
	{
		base._Ready();
	}

	public void Initialise(ItemStack itemStack, Vector2 position, Vector2 velocity)
	{
		this.itemStack = itemStack;
		GlobalPosition = position;
		Velocity = velocity;

		sprite2D.Texture = GD.Load(itemStack.GetItem().GetSpritePath()) as Texture2D;
		Enable();
	}

	public override bool Interact()
	{
		return true;
	}

	public override void Disengage()
	{
		base.Disengage();
		ItemStack remainingItems = playerInv.AddItem(itemStack);

		if (!remainingItems.IsEmpty())
			CS.EmitSignal("SpawnItem", remainingItems, Vector2.Inf, Vector2.Inf);

		QueueFree();
	}

	public override bool Engage(Player player)
	{
		base.Engage(player);
		return false;
	}

	public void SetInWater(bool inWater)
	{
		this.inWater = inWater;
	}

	public ItemStack GetItemStack()
	{
		return itemStack;
	}

	public override void _Process(double delta)
	{
		//foreach (InteractableItem item in itemsInRange)
			//if (item == null || item.IsQueuedForDeletion() || !IsInstanceValid(item))
				//itemsInRange.Remove(item);

		if (!IsQueuedForDeletion())
			MoveClosest(delta);

		if (inWater)
		{
			float discountFactor = 0.7f;
			Velocity = new Vector2(Velocity.X * discountFactor * (float)Math.Pow(discountFactor, delta), 30);
		}
		else if (!IsOnFloor())
		{
			Velocity = new Vector2(Velocity.X, (float) (Velocity.Y + 200 * delta));
			Disable();
		}
		else {
			Velocity = Vector2.Zero;
			Enable();
		}

		MoveAndSlide();
	}

	private void MoveClosest(double delta)
	{
		InteractableItem closestItem = null;
		
		foreach (InteractableItem item in itemsInRange)
		{
			if (item.GetItemStack().GetItem().GetType() == itemStack.GetItem().GetType()
			&& itemStack.GetItem().CanStack() && item.IsOnFloor() && IsOnFloor()
			&& !(item.GetItemStack().IsFullStack() || itemStack.IsFullStack()))
			{
				float distance = item.GlobalPosition.DistanceTo(GlobalPosition);
				float closestDistance = closestItem == null
					? float.MaxValue : closestItem.GlobalPosition.DistanceTo(GlobalPosition);

				if (distance < closestDistance)
					closestItem = item;
			}
		}

		if (closestItem != null)
			if (closestItem.GlobalPosition.DistanceTo(GlobalPosition) != 0)
				GlobalPosition = GlobalPosition.MoveToward(closestItem.GlobalPosition, 30 * (float) delta);
			else
				MergeStack(closestItem);
	}

	private void MergeStack(InteractableItem closeItem)
	{
		ItemStack newItemStack = closeItem.GetItemStack();
		int transferAmount = Math.Min(itemStack.GetItem().GetMaxStack() - itemStack.GetQuantity(), newItemStack.GetQuantity());
		
		newItemStack.ChangeQuantity(-transferAmount);
		itemStack.ChangeQuantity(transferAmount);
		
		if (newItemStack.IsEmpty())
		{
			itemsInRange.Remove(closeItem);
			closeItem.QueueFree();
		}
	}

	public void OnBodyEntered(Node2D body)
	{
		if (body is InteractableItem && body != this)
			itemsInRange.Add(body as InteractableItem);
	}

	public void OnBodyExited(Node2D body)
	{
		if (body is InteractableItem && body != this)
			itemsInRange.Remove(body as InteractableItem);
	}

	public override string GetPlayerAnimation()
	{
		return "idle";
	}
}
