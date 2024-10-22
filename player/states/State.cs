using Godot;
using System;
using System.Collections.Generic;

public abstract partial class State : Node
{
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