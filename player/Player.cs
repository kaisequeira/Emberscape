using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
	private AnimationPlayer Animator;
	private RayCast2D GroundDetection;

	private Interactable currInteractable = null;
	private State currentState = new Idle();
	private bool facingRight = true;

	public override void _Ready()
	{
		Animator = GetNode<AnimationPlayer>("AnimationPlayer");
		GroundDetection = GetNode<RayCast2D>("RayCast2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		SetDirectionFacing();
		currentState.UpdateInputs();
		currentState = currentState.Compute(this, delta);
		MoveAndSlide();
	}

    public override void _Input(InputEvent @event)
    {
		if (@event.IsActionPressed("numeric") && !(currentState is Interact)) {
			if (@event is InputEventKey eventKey) {
				//Select Slot logic ((int)eventKey.Keycode) % 4194390 - 49
			}
		}

		// change to include engage
		if (@event.IsActionPressed("interact") && !(currentState is Interact))
		{
			currentState = new Interact(this, currInteractable);
		}

		// add escape check for interactable - include disengage

		// add escape check for menu

		if (@event.IsActionPressed("drop") && !(currentState is Interact))
		{
			// item drop logic
		}
    }

	private void SetDirectionFacing()
	{ 
		if (((currentState.GetXInput() < 0 && facingRight) || (currentState.GetXInput() > 0 && !facingRight)) && (!(currentState is Interact))) 
		{
			Scale = new Vector2(Scale.X * -1, Scale.Y);
			facingRight = !facingRight;
		}
	}

	public void SetVelocity(Vector2 velocity)
	{
		Velocity = velocity;
	}

	public void SetVelocity(Nullable<float> x, Nullable<float> y)
	{
		Velocity = new Vector2(x ?? Velocity.X, y ?? Velocity.Y);
	}

	public Vector2 GetVelocity()
	{
		return Velocity;
	}

	public bool IsGrounded()
	{
		return GroundDetection.IsColliding();
	}

	public bool IsFacingRight()
	{
		return facingRight;
	}

	public void PlayAnimation(String animationName)
	{
		Animator.Play(animationName);
	}

}