using Godot;
using System;

public partial class Reeling : FishingStrategy
{
	private int numStages;

	private ButtonUI buttonPrompt;
	private string desiredInput = "commit";
	private string animName = "reeling";
	private int stage = 0;
	private bool leave = false;

	private float timeLimit;
	private Hook hook;

	public Reeling(FishingSign fishingSign, Hook hook) : base(fishingSign)
	{
		buttonPrompt = UI.Get().InstanceButton(
			fishingSign.GetGlobalTransformWithCanvas().Origin + new Vector2(0, -40), "Space"
		);
		numStages = (int)Mathf.Floor(GD.Randf() * 4) + 5;
		timeLimit = numStages * 0.5f;

		hook.Velocity = Vector2.Zero;
		this.hook = hook;
	}

	public override void ComputeStrategy()
	{
		if (leave)
			return;

		if (Input.IsActionJustPressed(desiredInput))
		{
			if (stage < numStages)
			{
				stage++;
			}
	
			if (stage >= numStages)
			{
				animName = "reeling - pull";
				
				CloseStrategy();
				leave = true;
				return;
			}
		}

		if (Input.IsActionPressed(desiredInput))
		{
			buttonPrompt.SetClicked(true);
		}
		else
		{
			buttonPrompt.SetClicked(false);
		}

		if (timeLimit <= 0)
		{
			fishingSign.TransitionToWade();
			return;
		}

		Random random = new Random();
		if (random.NextDouble() <= 0.1)
		{
			hook.Velocity = new Vector2((float)(random.NextDouble() * 80 - 40), hook.Velocity.Y);
		}
	}

	public override void CloseStrategy()
	{
		if (IsInstanceValid(buttonPrompt))
			buttonPrompt?.QueueFree();
	}

	public override string GetAnimation()
	{
		return animName;
	}

	public void IncrementTimePassed(double delta)
	{
		timeLimit -= (float) delta;
	}
}
