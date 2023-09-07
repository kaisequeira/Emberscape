using Godot;
using System;

public partial class Idle : State
{
    public override State Compute(Player player, double delta)
    {
		if (xInput != 0)
		{
			return new Move();
		}
		else
		{
			player.SetVelocity(Vector2.Zero);
		}

		player.PlayAnimation("idle");
		return this;
    }
}