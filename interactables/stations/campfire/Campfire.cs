using Godot;
using System;

public partial class Campfire : Station
{
	private const float waitTime = 1f;
	private bool isLit = false;
	private float burnTime = 0;

	public override void Initialise(Tile tile)
	{
		base.Initialise(tile);
	}

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		if (!isLit)
			if (burnTime >= waitTime)
			{
				isLit = true;
				GetParentTile().LightScene();
			}
			else
				burnTime += (float) delta;
    }

    public override void _Ready()
	{
		base._Ready();
	}

    public override bool Interact()
	{
		return true;
	}

    public override void Disengage()
    {
		base.Disengage();
		UI.Get().GetInventory(Inventory.Types.Campfire).Disable();
    }

    public override bool Engage(Player player)
    {
		base.Engage(player);
		UI.Get().GetInventory(Inventory.Types.Campfire).Enable();
		return true;
    }

	public void OnAnimationFinished(string prevAnimation)
	{
		if (prevAnimation == "Start")
		{
			PlayAnimation("Burn");
		}
	}

	public override string GetPlayerAnimation()
	{
		return "idle";
	}
}