using Godot;
using System;

public abstract partial class Fuel : Item
{
    protected int fuelValue;

    public Fuel(int maxStack, int fuelValue, string spritePath) : base(maxStack, spritePath)
    {
        this.fuelValue = fuelValue;
    }

    public int GetFuelValue()
    {
        return fuelValue;
    }
}