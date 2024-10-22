using Godot;
using System;

public partial class FishingSign : Station
{
	[Export]
	private bool faceRight = false;

	[Export]
	private Node2D leftBound;

	[Export]
	private Node2D rightBound;

	private Type[] fishClasses = { typeof(Pointfish), typeof(Anglenose), typeof(Bluntnose), typeof(Spadefish) };

	private FishingStrategy currentState;
	private PackedScene hookScene;
	private Hook hook;

	private bool inFishing = false;
	private bool leave = false;

	private Type chosenFishType;

	public override void _Ready()
	{
		base._Ready();
		AssignRandomFish();
		AnimationPlayer.GetAnimation("Start").TrackSetKeyValue(0, 0, GD.Load<Texture>("res://art/interactables/fishing/start - " + chosenFishType.Name + ".png"));
		hookScene = GD.Load<PackedScene>("res://interactables/stations/fishing/Hook.tscn");

		if (faceRight)
		{	
			Scale = new Vector2(Scale.X * (faceRight ? -1 : 1), Scale.Y);
		}
	}

    public override void _Process(double delta)
    {
        base._Process(delta);

		if (currentState is Wading)
		{
			(currentState as Wading).UpdateTimePressed(delta);
		}
		else if (currentState is Reeling)
		{
			(currentState as Reeling).IncrementTimePassed(delta);
		}
    }

	private void AssignRandomFish()
	{
        Type[] fishClasses = { typeof(Pointfish), typeof(Anglenose), typeof(Bluntnose), typeof(Spadefish) };
        int randomIndex = (int)Mathf.Floor(GD.Randf() * fishClasses.Length);
        chosenFishType = fishClasses[randomIndex];
	}

    public override bool Interact()
	{
		if (inFishing && currentState != null)
		{
			currentState.ComputeStrategy();
		}

		player.DrawFishingLine(hook, faceRight);
		return !leave;
	}

    public override void Disengage()
    {
		base.Disengage();
		if (IsInstanceValid(hook))
		{
			hook?.QueueFree();
			hook = null;
		}

		currentState.CloseStrategy();
		currentState = null;
		inFishing = false;
		player.DrawFishingLine(hook, faceRight);
    }

    public override bool Engage(Player player)
    {
		base.Engage(player);
		player.ConnectAnimFinishToMethod("PlayerAnimationFinished", this);

		if (faceRight && !player.IsFacingRight())
		{
			player.SetDirectionFacing(true);
		}
		else if (!faceRight && player.IsFacingRight())
		{
			player.SetDirectionFacing(false);
		}

		currentState = new Casting(this);
		inFishing = true;
		leave = false;

		return true;
    }

	public override string GetPlayerAnimation()
	{
		return currentState.GetAnimation();
	}

	public bool GetFacingRight()
	{
		return faceRight;
	}

	public void PlayerAnimationFinished(string animName)
	{
		if (!inFishing)
			return;

		if (animName == "casting")
		{
			hook = hookScene.Instantiate() as Hook;
			AddChild(hook);
			hook.Initialise(player.GlobalPosition + new Vector2((faceRight ? 1 : -1) * 10, -20), new Vector2((faceRight ? 1 : -1) * 150, -35), this, leftBound.GlobalPosition, rightBound.GlobalPosition);
			(currentState as Casting)?.ChangeCurrentAnimation("casting - held");
		}
		else if (animName == "reeling - pull")
		{
			// calculations to determine throw velocity
			Vector2 throwVelocity = new Vector2(faceRight ? -100 : 100, -165);
			CS.EmitSignal("SpawnItem", new ItemStack(Activator.CreateInstance(chosenFishType) as Item, (int)Mathf.Floor(GD.Randf() * 2) + 1), throwVelocity, hook.GlobalPosition);
			hook.Velocity = throwVelocity;
			leave = true;	
		}
	}

	public void TransitionToWade()
	{
		currentState.CloseStrategy();
		currentState = new Wading(this, hook);
	}

	public void TransitionToReeling()
	{
		currentState.CloseStrategy();
		currentState = new Reeling(this, hook);
	}
}