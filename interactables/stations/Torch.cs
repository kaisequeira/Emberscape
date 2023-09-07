using Godot;
using System;

public partial class Torch : Station
{
	public override void _Ready()
	{
		base._Ready();
	}

	public override void _Process(double delta)
	{

	}

    public override void Interact()
	{
		
	}

    public override void Disengage(Player player)
    {

    }

    public override void Engage(Player player)
    {
		GetParentTile().LightScene();
    }

	private void SetTorchColliderPos()
	{
		int tileOffset = (int)Mathf.Floor(GlobalPosition.X / TYPES.TILE_SIZE.X);
		GlobalPosition = new Vector2(TYPES.TILE_SIZE.X/2 + tileOffset * TYPES.TILE_SIZE.X, 100);
	}

	public void OnAnimationFinished(string prevAnimation)
	{
		if (prevAnimation == "Start")
		{
			PlayAnimation("Burn");
		}
	}
}