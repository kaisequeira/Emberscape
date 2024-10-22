using Godot;
using System;

public abstract partial class Consumable : Item
{
    protected int consumeValue;

    public Consumable(int maxStack, int consumeValue, string spritePath) : base(maxStack, spritePath)
    {
        this.consumeValue = consumeValue;
    }

    public int GetConsumeValue()
    {
        return consumeValue;
    }
}