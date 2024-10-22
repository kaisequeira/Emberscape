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

	private Dictionary<string, float> modifiers = new Dictionary<string, float>();

	public override void _Ready()
	{
		Animator = GetNode<AnimationPlayer>("AnimationPlayer");
		GroundDetection = GetNode<RayCast2D>("RayCast2D");
		CS = GetNode<CustomSignals>("/root/CustomSignals");

		playerInv = UI.Get().GetInventory(Inventory.Types.Player);
		playerInv.SelectSlot(selectedSlot);
	}

	public override void _PhysicsProcess(double delta)
	{	
		UpdateClosestInteractable();

		currentState.UpdateInputs();
		currentState = currentState.Compute(this, delta);

		if (!(currentState is Interact))
			UpdateDirectionFacing();

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
				curInteractable.Disengage();
			}
		}

		// Handle menu
		if (@event.IsActionPressed("escape") && !(currentState is Interact))
		{

		}

		// Handle item drop
		if (@event.IsActionPressed("drop") && !(currentState is Interact))
		{
			ItemStack itemStack = Inventory.DropSlot(playerInv.GetSlots()[selectedSlot], true);

			if (!itemStack.IsEmpty())
			{
				CS.EmitSignal(CustomSignals.SignalName.SpawnItem, itemStack, Vector2.Inf, Vector2.Inf);
			}
		}

		// Consume item
		if (@event.IsActionPressed("consume") && !(currentState is Interact))
		{
			Slot consumeSlot = playerInv.GetSlots()[selectedSlot];
			ItemStack itemStack = new ItemStack(consumeSlot.GetItem(), consumeSlot.GetQuantity());

			if (!itemStack.IsEmpty() && itemStack.GetItem() is Consumable)
			{
				Inventory.DropSlot(consumeSlot, false);
				CS.EmitSignal(CustomSignals.SignalName.ConsumeFood, itemStack.GetQuantity() * ((Consumable) itemStack.GetItem()).GetConsumeValue());
			}
		}
	}

    public float GetModifiersProduct()
    {
        float product = 1.0f;
        foreach (var modifier in modifiers.Values)
        {
            product *= modifier;
        }
        return product;
    }

    public void AddModifier(string key, float value)
    {
        if (modifiers.ContainsKey(key))
            modifiers[key] = value;
        else
            modifiers.Add(key, value);
    }

    public void RemoveModifier(string key)
    {
        if (modifiers.ContainsKey(key))
            modifiers.Remove(key);
        else
			return;
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

	private void UpdateDirectionFacing()
	{
		if (currentState.GetXInput() < 0 && facingRight) 
		{
			SetDirectionFacing(false);
		}
		else if (currentState.GetXInput() > 0 && !facingRight)
		{
			SetDirectionFacing(true);
		}
	}

	public void SetDirectionFacing(bool faceRight)
	{
		Scale = new Vector2(Scale.Y * (faceRight ? 1 : -1), Scale.Y);
		facingRight = faceRight;
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

		if (!(currentState is Interact) && IsInstanceValid(curInteractable))
			curInteractable?.HighlightOff();
		
		curInteractable = newClosestInteractable;
		
		if (!(currentState is Interact))
			curInteractable?.HighlightOn();
	}

	public void PlayAnimation(string animationName)
	{
		Animator.Play(animationName);
	}

	public string GetAnimation()
	{
		return Animator.CurrentAnimation;
	}

	public void ConnectAnimFinishToMethod(string functionName, Node node)
	{
		if (!Animator.IsConnected("animation_finished", new Callable(node, functionName)))
			Animator.Connect("animation_finished", new Callable(node, functionName));
	}

	public void DrawFishingLine(Hook hook, bool faceRight)
	{
		GetNode<FishingLine>("FishingLine").DrawFishingLine(hook, faceRight);
	}
}