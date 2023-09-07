using Godot;
using System;

public partial class TYPES : Node
{
	public static readonly Vector2 TILE_SIZE = new Vector2(256, 144);

	public enum STATES
	{
		IDLE = 0,
		MOVE = 1,
		INTERACT = 2,
	}
}
