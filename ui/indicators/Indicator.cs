using Godot;
using System;

public partial class Indicator : TextureRect
{
    private float animTime = 0.1f;
	private float curTime = 0;
	private int curAnim = 1;

    [Export]
    private int frames;

	public override void _Process(double delta)
	{
		curTime += (float) delta;

		if (curTime >= animTime)
		{
			curTime = 0;
			curAnim++;
			
			if (curAnim > frames)
				curAnim = 1;

			Texture = (Texture2D) GD.Load($"res://art/ui/indicators/{Name.ToString().Substring(0, Name.ToString().IndexOf("Indicator")).ToLower()}/{Name}{curAnim}.png");
		}
	}

	public void SetAnimSpeed(float speed)
	{
		animTime = speed;
	}
}
