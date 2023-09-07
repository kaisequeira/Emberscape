using Godot;
using System;

public abstract partial class State : Node
{
	protected const float ACCELERATION = 35.0f;
	protected const float MAX_SPEED = 85.0f;
	protected const float MIN_SPEED = 45.0f;

	protected float xInput;
	protected float yInput;

	public abstract State Compute(Player player, double delta);

	public void UpdateInputs()
	{
		xInput = Input.GetAxis("left", "right");
		yInput = Input.GetAxis("down", "up");
	}

	public float GetXInput()
	{
		return xInput;
	}

	public float GetYInput()
	{
		return yInput;
	}
}