using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
	private AnimationPlayer Animator;
	private RayCast2D GroundDetection;
	private CustomSignals CS;

	private List<Interactable> interactables = new List<Interactable>();
	private Interactable curInteractable = null;

	private int selectedSlot = 0;
	private State currentState = new Idle();
	private bool facingRight = true;
	private Inventory playerInv;

	public override void _Ready()
	{
		Animator = GetNode<AnimationPlayer>("AnimationPlayer");
		GroundDetection = GetNode<RayCast2D>("RayCast2D");
		CS = GetNode<CustomSignals>("/root/CustomSignals");

		playerInv = InvManager.Get().GetInventory(Inventory.Types.Player);
		playerInv.SelectSlot(selectedSlot);
	}

	public override void _PhysicsProcess(double delta)
	{	
		UpdateClosestInteractable();

		currentState.UpdateInputs();
		currentState = currentState.Compute(this, delta);

		SetDirectionFacing();
		MoveAndSlide();
	}

    public override void _Input(InputEvent @event)
    {
		// Handle select slot
		if (@event.IsActionPressed("numeric") && !(currentState is Interact))
		{
			if (@event is InputEventKey eventKey)
			{
				int inputSlot = ((int)eventKey.Keycode) % 4194390 - 49;
				selectedSlot = Math.Min(inputSlot, playerInv.GetSlots().Count - 1);
				playerInv.SelectSlot(selectedSlot);
			}
		}

		// Handle interactions
		if (@event.IsActionPressed("interact") && !(currentState is Interact) && curInteractable != null)
		{
			if (curInteractable != null && curInteractable.Engage(this))
			{
				currentState = new Interact(this, curInteractable);
			}
			else if (curInteractable != null)
			{
				curInteractable.Disengage(this);
			}
		}

		// Handle menu
		if (@event.IsActionPressed("escape") && !(currentState is Interact))
		{

		}

		// Handle item drop
		if (@event.IsActionPressed("drop") && !(currentState is Interact))
		{
			ItemStack itemStack = Inventory.DropSlot(playerInv.GetSlots()[selectedSlot]);

			if (!itemStack.IsEmpty())
			{
				CS.EmitSignal("SpawnItem", itemStack, new Vector2(), Vector2.Inf);
			}
		}
	}

	public int GetSelected()
	{
		return selectedSlot;
	}

	public Vector2 GetVelocity()
	{
		return Velocity;
	}

	public void SetVelocity(Vector2 velocity)
	{
		Velocity = velocity;
	}

	public void SetVelocity(Nullable<float> x, Nullable<float> y)
	{
		Velocity = new Vector2(x ?? Velocity.X, y ?? Velocity.Y);
	}

	private void SetDirectionFacing()
	{ 
		if (((currentState.GetXInput() < 0 && facingRight) || (currentState.GetXInput() > 0 && !facingRight)) && (!(currentState is Interact))) 
		{
			Scale = new Vector2(Scale.X * -1, Scale.Y);
			facingRight = !facingRight;
		}
	}

	public bool IsFacingRight()
	{
		return facingRight;
	}

	public bool IsGrounded()
	{
		return GroundDetection.IsColliding();
	}

	public void AddInteractable(Interactable interactable)
	{
		interactables.Add(interactable);
	}

	public void RemoveInteractable(Interactable interactable)
	{
		interactables.Remove(interactable);
	}

	private void UpdateClosestInteractable()
	{
		Interactable newClosestInteractable = interactables
			.Where(interactable => interactable.CanInteract())
			.OrderBy(interactable => GlobalPosition.DistanceTo(interactable.GetPosition()))
			.FirstOrDefault();

		if (curInteractable != newClosestInteractable)
		{
			if (IsInstanceValid(curInteractable)) curInteractable?.HighlightOff();
			curInteractable = newClosestInteractable;
			curInteractable?.HighlightOn();
		}
	}

	public void PlayAnimation(String animationName)
	{
		Animator.Play(animationName);
	}

}