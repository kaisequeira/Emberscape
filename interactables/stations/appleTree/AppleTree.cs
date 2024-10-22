using Godot;
using System.Linq;
using System.Collections.Generic;

public partial class AppleTree : Station
{
	private Node2D AppleList;
	private ButtonUI buttonPrompt;
	private Timer GrowthTimer;

	private List<FallingApple> apples = new List<FallingApple>();
	private bool canPick = false;
	private bool shaking = false;
	private int spawnNumLeft;

	public override void _Ready()
	{
		base._Ready();
		AppleList = GetNode<Node2D>("AppleList");
		GrowthTimer = GetNode<Timer>("Timer");
		
		foreach(FallingApple apple in AppleList.GetChildren().Cast<FallingApple>())
			apples.Add(apple);
	}

    public override bool Interact()
	{
		if (spawnNumLeft == 0 && apples.Count - GetNumSpawnableSlots().Count == 0)
		{
			GrowthTimer.WaitTime = GD.RandRange(15, 35);
			GrowthTimer.Start();
			Disable();
			return false;
		}

		if (GetNumSpawnableSlots().Count == 0 || shaking || spawnNumLeft == 0)
		{
			return true;
		}
		else if (buttonPrompt == null || !IsInstanceValid(buttonPrompt))
		{
			buttonPrompt = UI.Get().InstanceButton(GetGlobalTransformWithCanvas().Origin + new Vector2(38, -21.75f), "Space");
		}	

		if (Input.IsActionJustPressed("commit"))
		{
			shaking = true;
			PlayAnimation("Shake");

			if (IsInstanceValid(buttonPrompt))
				buttonPrompt?.QueueFree();
		}

		if (Input.IsActionPressed("commit"))
		{
			buttonPrompt.SetClicked(true);
		}
		else
		{
			buttonPrompt.SetClicked(false);
		}

		return true;
	}

    public override void Disengage()
    {
		base.Disengage();
		if (IsInstanceValid(buttonPrompt))
			buttonPrompt?.QueueFree();

		canPick = false;
    }

    public override bool Engage(Player player)
    {
		base.Engage(player);
		if (spawnNumLeft == 0 && apples.Count - GetNumSpawnableSlots().Count == 0)
			spawnNumLeft = GD.RandRange(1, apples.Count);
		canPick = true;
		return true;
    }

	public override string GetPlayerAnimation()
	{
		return "idle";
	}

	public bool isPickable()
	{
		return canPick && spawnNumLeft == 0;
	}

	public void OnAnimationFinished(string animName)
	{
		if (animName == "Shake")
		{
			List<FallingApple> availableApples = GetNumSpawnableSlots();
			FallingApple randomApple = availableApples[GD.RandRange(0, availableApples.Count - 1)];
			randomApple.Enable(this);
			spawnNumLeft--;
			shaking	= false;
		}
	}

	private List<FallingApple> GetNumSpawnableSlots()
	{
		return apples.FindAll(apple => !apple.Visible);
	}
}