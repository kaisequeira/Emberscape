using Godot;
using System;

public partial class ItemStack : Panel
{
    private Item item;
    private int quantity;

    public ItemStack(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public Item GetItem()
    {
        return item;
    }

    public int GetQuantity()
    {
        return quantity;
    }

    public void ChangeQuantity(int change)
    {
        this.quantity += change;
    }

    public bool IsEmpty()
    {
        return item == null || quantity <= 0;
    }

    public bool IsFullStack()
    {
        return quantity >= item.GetMaxStack();
    }
}