using Godot;
using System;

public partial class AppleTree : Station
{
	public override void _Ready()
	{
		base._Ready();
	}

    public override void Interact()
	{
		
	}

    protected override void Disengaged(Player player)
    {

    }

    protected override bool Engaged(Player player)
    {
		return true;
    }
}