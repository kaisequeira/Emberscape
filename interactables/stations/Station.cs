using Godot;
using System.Collections.Generic;

public abstract partial class Station : Interactable
{
	private Tile tile;
	private AnimationPlayer AnimationPlayer;

	public virtual void Initialise(Tile tile)
	{
		this.tile = tile;
	}

	public override void _Ready()
	{
		base._Ready();
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public void Light()
	{
		PlayAnimation("Start");
	}

	public Tile GetParentTile()
	{
		return tile;
	}

	public void PlayAnimation(string animationName)
	{
		AnimationPlayer.Play(animationName);
	}
}
