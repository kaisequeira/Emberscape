using Godot;
using System;

public partial class InteractableItem : Interactable
{
    private ItemStack itemStack;

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

    public override void Interact()
	{
		// Empty
	}

    protected override void Disengaged(Player player)
    {
        QueueFree();
    }

    protected override bool Engaged(Player player)
    {
        InvManager.Get().GetInventory(Inventory.Types.Player).AddItem(itemStack);
        return false;
    }

    public override void _Process(double delta)
    {
        if (!IsOnFloor())
        {
            Velocity = new Vector2(Velocity.X, (float) (Velocity.Y + 200 * delta));
        }
        else{
            Velocity = Vector2.Zero;
        }
        MoveAndSlide();
    }
}
