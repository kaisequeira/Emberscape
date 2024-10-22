using Godot;
using System;
using System.Collections.Generic;

public partial class Move : State
{
	private const float ACCELERATION = 35.0f;
	private const float MAX_SPEED = 85.0f;
	private const float MIN_SPEED = 45.0f;

	private float product;

    private float Acceleration
    {
        get { return ACCELERATION * product; }
    }

    private float MaxSpeed
    {
        get { return MAX_SPEED * product; }
    }

    private float MinSpeed
    {
        get { return MIN_SPEED * product; }
    }

    public override State Compute(Player player, double delta)
    {
		product = player.GetModifiersProduct();

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
			float targetVelX = player.GetVelocity().X + (float)(xInput * Acceleration * delta);
			float clampedX = Math.Clamp(targetVelX, (xInput > 0) ? MinSpeed : -MaxSpeed, (xInput > 0) ? MaxSpeed : -MinSpeed);
			player.SetVelocity(clampedX, null);
		}
		else
		{
			player.SetVelocity(Mathf.MoveToward(player.GetVelocity().X, 0, Acceleration), null);
		}

		player.PlayAnimation("walking");
		return this;
    }
}