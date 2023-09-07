using Godot;
using System;

public partial class Move : State
{
    public override State Compute(Player player, double delta)
    {
		if (player.GetVelocity() == Vector2.Zero && xInput == 0)
		{
			return new Idle();
		}
        else if (!player.IsGrounded())
		{
			player.SetVelocity(Vector2.Zero);
		}
		else if (xInput != 0)
		{
			player.SetVelocity(Math.Clamp(player.GetVelocity().X + (float)(xInput * ACCELERATION * delta),
				(xInput > 0) ? MIN_SPEED : -MAX_SPEED, (xInput > 0) ? MAX_SPEED : -MIN_SPEED), null);
		}
		else
		{
			player.SetVelocity(Mathf.MoveToward(player.GetVelocity().X, 0, ACCELERATION), null);
		}

		player.PlayAnimation("walking");
		return this;
    }
}