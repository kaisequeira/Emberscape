using Godot;
using System;

public partial class Torch : Station
{
	public override void _Ready()
	{
		base._Ready();
		Enable();
	}

    public override void Interact()
	{
		
	}

    protected override void Disengaged(Player player)
    {

    }

    protected override bool Engaged(Player player)
    {
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
}