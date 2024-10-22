using Godot;
using System;

public partial class FishingLine : Node2D
{
	private Hook hook = null;
	private bool faceRight;

	public void DrawFishingLine(Hook hook, bool faceRight)
	{
		this.faceRight = faceRight;
		this.hook = hook;
		QueueRedraw();
	}

    public override void _Draw()
    {
        base._Draw();

		if (hook == null || !IsInstanceValid(hook))
			return;

		DrawLine(
			Vector2.Zero,
			new Vector2((faceRight ? -1 : 1) * (GlobalPosition.X - hook.GlobalPosition.X), hook.GlobalPosition.Y - GlobalPosition.Y),
			Colors.White
		);
    }
}
