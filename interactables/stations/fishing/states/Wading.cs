using Godot;
using System;

public partial class Wading : FishingStrategy
{
	private Hook hook;
	private ButtonUI buttonPrompt;

	private State state = State.Loose;
	private State desiredState = State.Taught;

	private float requiredTime;
	private float currentTime = 0;

	private string looseKey;
	private string taughtKey;

	private int requiredWades;
	private int currentWades = 0;

	private enum State
	{
		Taught,
		Loose
	}

	public Wading(FishingSign fishingSign, Hook hook) : base(fishingSign)
	{
		this.hook = hook;
		taughtKey = fishingSign.GetFacingRight() ? "A" : "D";
		looseKey = fishingSign.GetFacingRight() ? "D" : "A";

		buttonPrompt = UI.Get().InstanceButton(
			fishingSign.GetGlobalTransformWithCanvas().Origin + new Vector2(0, -40), desiredState == State.Loose ? looseKey : taughtKey
		);

		requiredTime = GD.Randf() * 2.5f + 0.5f;
		requiredWades = (int)Mathf.Floor(GD.Randf() * 2) + 2;
	}

    public override void ComputeStrategy()
	{
		if (Input.GetAxis("left", "right") > 0)
		{
			state = taughtKey == "D" ? State.Taught : State.Loose;
			hook.Velocity = Vector2.Right * 15;
		}
		else if (Input.GetAxis("left", "right") < 0)
		{
			state = taughtKey == "A" ? State.Taught : State.Loose;
			hook.Velocity = Vector2.Left * 15;
		}
		else
		{
			hook.Velocity = Vector2.Zero;
		}

		if (Input.GetAxis("left", "right") == 0)
		{
			buttonPrompt.SetClicked(false);
		}
		else
		{
			if (state == desiredState)
			{
				buttonPrompt.SetClicked(true);
			}
			else
			{
				buttonPrompt.SetClicked(false);
			}
		}

		if (currentTime >= requiredTime)
		{
			requiredTime = (int)Mathf.Floor(GD.Randf() * 2) + 2;
			currentTime = 0;
			currentWades++;
			desiredState = desiredState == State.Loose ? State.Taught : State.Loose;
			buttonPrompt.SetText(desiredState == State.Loose ? looseKey : taughtKey);
		}

		if (currentWades >= requiredWades)
		{
			fishingSign.TransitionToReeling();
		}
	}

	public void UpdateTimePressed(double delta)
	{
		if (state == desiredState)
		{
			currentTime += (float) delta;
		}
	}

	public override void CloseStrategy()
	{
		if (IsInstanceValid(buttonPrompt))
			buttonPrompt?.QueueFree();
	}

	public override string GetAnimation()
	{
		return state == State.Loose ? "wading - loose" : "wading - taught";
	}
}
