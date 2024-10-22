using Godot;
using System;

public partial class Hook : CharacterBody2D
{
	private FishingSign fishingSign;
	private bool inWater = false;

	private Vector2 leftBound;
	private Vector2 rightBound;

	private float yCoord;

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
	{
		if (!IsOnFloor() && !inWater)
		{
            Velocity = new Vector2(Velocity.X, (float) (Velocity.Y + 200 * delta));
		}
		else if (inWater)
		{
			GlobalPosition = new Vector2(Mathf.Clamp(GlobalPosition.X, leftBound.X, rightBound.X), GlobalPosition.Y);
			GlobalPosition = new Vector2(GlobalPosition.X, yCoord);
		}
		MoveAndSlide();
	}

	public void Initialise(Vector2 position, Vector2 velocity, FishingSign fishingSign, Vector2 leftBound, Vector2 rightBound)
	{
		this.fishingSign = fishingSign;
		GlobalPosition = position;
		Velocity = velocity;
		this.leftBound = leftBound;
		this.rightBound = rightBound;
	}

	public void TransitionToWade()
	{
		inWater = true;
		yCoord = GlobalPosition.Y;
		Velocity = Vector2.Zero;
		fishingSign.TransitionToWade();
	}
}
