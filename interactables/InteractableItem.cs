using Godot;
using System;

public partial class InteractableItem : Interactable
{
    private Item item;

	public void Initialise(Item item)
	{
		this.item = item;
	}

    public override void Interact()
	{
		// empty
	}

    public override void Disengage(Player player)
    {
        // QueueFree(this)
    }

    public override void Engage(Player player)
    {
        // player.GetInventory.AddItem(item);
        // player.disengage(this) etc.
    }
}
