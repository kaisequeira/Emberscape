using Godot;
using System;

public partial class Interact : State
{
	private Interactable interactable;

	public Interact(Player player, Interactable interactable)
	{
		this.interactable = interactable;
		interactable.Engage(player);
	}

    public override State Compute(Player player, double delta)
    {
		player.SetVelocity(Vector2.Zero);
		interactable.Interact();

		if (Input.IsActionJustPressed("escape"))
		{
			interactable.Disengage(player);
			return new Idle();
		}

		player.PlayAnimation("idle");
		return this;
    }
}