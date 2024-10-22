using Godot;
using System;

public partial class Casting : FishingStrategy
{
	private ButtonUI buttonPrompt;
	private string desiredInput = "commit";
	private string currentAnim = "idle";

	public Casting(FishingSign fishingSign) : base(fishingSign)
	{
		buttonPrompt = UI.Get().InstanceButton(fishingSign.GetGlobalTransformWithCanvas().Origin + new Vector2(0, -40), "Space");
	}

	public override void ComputeStrategy()
	{
		if (Input.IsActionPressed(desiredInput))
		{
			buttonPrompt.SetClicked(true);
		}
		else if (currentAnim != "idle")
		{
			CloseStrategy();
		}
		else
		{
			buttonPrompt.SetClicked(false);
		}

		if (Input.IsActionJustPressed(desiredInput) && currentAnim == "idle")
		{
			currentAnim = "casting";
		}
	}

	public override void CloseStrategy()
	{
		if (IsInstanceValid(buttonPrompt))
			buttonPrompt?.QueueFree();
	}

	public override string GetAnimation()
	{
		return currentAnim;
	}

	public void ChangeCurrentAnimation(string anim)
	{
		currentAnim = anim;
	}
}
