using Godot;
using System;

public partial class Torch : Station
{
	public override void _Ready()
	{
		base._Ready();
		Enable();
	}

    public override bool Interact()
	{
		return true;
	}

    public override void Disengage()
    {
		base.Disengage();
    }

    public override bool Engage(Player player)
    {
		base.Engage(player);
		GetParentTile().LightScene();
		return false;
    }

	private void SetTorchColliderPos()
	{
		int tileOffset = (int)Mathf.Floor(GlobalPosition.X / Tile.Size.X);
		GlobalPosition = new Vector2(Tile.Size.X/2 + tileOffset * Tile.Size.X, 100);
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