using Godot;
using System;

public abstract partial class FishingStrategy : Node
{
	public abstract void ComputeStrategy();

	public abstract void CloseStrategy();

	public abstract string GetAnimation();

	protected FishingSign fishingSign;

	public FishingStrategy(FishingSign fishingSign)
	{
		this.fishingSign = fishingSign;
	}
}
