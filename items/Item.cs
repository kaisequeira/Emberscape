using Godot;
using System;

public abstract partial class Item : Node
{
    private int maxStack;

    private string spritePath;

    public Item(int maxStack, string spritePath)
    {
        this.maxStack = maxStack;
        this.spritePath = spritePath;
    }

    public bool CanStack() {
        return maxStack > 1;
    }

    public int GetMaxStack() {
        return maxStack;
    }

    public string GetSpritePath() {
        return spritePath;
    }

    public override bool Equals(object obj)
    {
        if (obj is Item otherItem)
        {
            return this.GetType() == otherItem.GetType();
        }
        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}