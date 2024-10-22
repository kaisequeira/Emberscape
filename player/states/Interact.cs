using Godot;
using System;

public partial class Interact : State
{
	private Interactable interactable;

	public Interact(Player player, Interactable interactable)
	{
		this.interactable = interactable;
	}

    public override State Compute(Player player, double delta)
    {
		player.SetVelocity(Vector2.Zero);

		if (!interactable.Interact() || Input.IsActionJustPressed("escape"))
		{
			interactable.Disengage();
			return new Idle();
		}

		player.PlayAnimation(interactable.GetPlayerAnimation());
		return this;
    }
}