using Godot;
using System;

public partial class FallenTree : Station
{
	private static readonly int NUMSTAGES = 6;

	private ButtonUI buttonPrompt;
	private string desiredInput = "commit";
	private int stage = 0;
	private bool leave = false;
	private GpuParticles2D particles;

	public override void _Ready()
	{
		base._Ready();
		particles = GetNode<GpuParticles2D>("GPUParticles2D");
	}

    public override bool Interact()
	{
		if (Input.IsActionJustPressed(desiredInput))
		{
			if (stage < NUMSTAGES)
			{
				stage++;
			}
	
			if (stage == NUMSTAGES - 1)
			{
				desiredInput = "interact";
				buttonPrompt.SetText("E");
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

		return !leave;
	}

    public override void Disengage()
    {
		base.Disengage();

		if (player.GetAnimation() == "axe" + NUMSTAGES && !leave)
		{
			CS.EmitSignal("SpawnItem", new ItemStack(new Log(), 1), Vector2.Inf, Vector2.Inf);
		}

		ResetFallenTree();
    }

    public override bool Engage(Player player)
    {
		base.Engage(player);
		player.ConnectAnimFinishToMethod("PlayerAnimationFinished", this);
		buttonPrompt = UI.Get().InstanceButton(GetGlobalTransformWithCanvas().Origin + new Vector2(21.5f, -32), "Space");
		return true;
    }

	public override string GetPlayerAnimation()
	{
		return "axe" + stage;
	}

	public void PlayerAnimationFinished(string animName)
	{
		if (animName == "axe" + NUMSTAGES)
		{
			particles.Emitting = true;
			CS.EmitSignal("SpawnItem", new ItemStack(new Log(), 1), Vector2.Inf, Vector2.Inf);
			leave = true;
		}
	}

	private void ResetFallenTree()
	{
		buttonPrompt?.QueueFree();
		leave = false;
		stage = 0;
		desiredInput = "commit";
	}
}