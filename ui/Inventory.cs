using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Inventory : Control
{ 
    public static readonly int INVALID_SLOT = -1;

	public enum Types
	{
		Player,
		Mystery,
		Crate,
		Logpile,
	}

    [Export]
    private Types type;

    [Export]
    private int slotCount;

    [Export]
    private int columns;

    private List<Slot> slots = new List<Slot>();
    private GridContainer gridContainer;

    public override void _Ready()
    {
        gridContainer = GetNode<GridContainer>("GridContainer");
        gridContainer.Columns = columns;
        IncreaseSize(slotCount);

        if (type != Types.Player)
            Disable();
        else
            foreach (Slot slot in slots)
                slot.SetItemStack(new ItemStack(new Firewood(), 2));
    }

    public List<Slot> GetSlots()
    {
        return slots;
    }

    public Types GetInvType()
    {
        return type;
    }

    public void SelectSlot(int slot)
    {
        foreach (Slot s in slots)
            s.SetSelected(false);

        if (slot != INVALID_SLOT)
            slots[slot].SetSelected(true);
    }

    public void Disable()
    {
        InvManager.Get().SetAltInv(null);
        Visible = false;
    }

    public void Enable()
    {
        InvManager.Get().SetAltInv(this);
        Visible = true;
    }

    public void IncreaseSize(int newSlots)
    {
        for (int i = 0; i < newSlots; i++)
        {
            Slot slot = GD.Load<PackedScene>("res://ui/Slot.tscn").Instantiate<Slot>();
            slots.Add(slot);
            gridContainer.AddChild(slot);
            slot.SetItemStack(new ItemStack(null, 0));
        }
    }

    public void UseItem(ItemStack itemStack)
    {   
        foreach (Slot slot in slots)
        {
            if (slot.IsEmpty())
                continue;

            if (slot.GetItem().Equals(itemStack.GetItem()))
            {
                int quantityToUse = Math.Min(slot.GetQuantity(), itemStack.GetQuantity());
                slot.ChangeQuantity(-quantityToUse);
                itemStack.ChangeQuantity(-quantityToUse);
            }

            if (itemStack.GetQuantity() == 0)
                return;
        }
    }

    public bool UseItems(List<ItemStack> itemStacks) 
    {
        foreach (ItemStack itemStack in itemStacks)
        {
            if (itemStack.GetQuantity() > slots.Where(slot => !slot.IsEmpty()
                && slot.GetItem().Equals(slot.GetItem()))
                .Sum(slot => slot.GetQuantity()))
                return false;
        }

        foreach (ItemStack itemStack in itemStacks)
            UseItem(itemStack);

        return true;
    }

    public void TransferSlot(Slot slot)
    {
        if (slot.IsEmpty())
            return;

        // If the other inventory is full, don't transfer
        if (InvManager.Get().GetAltInv(this).GetSlots()
            .Where(altSlot => altSlot.IsEmpty() || (altSlot.GetItem().Equals(slot.GetItem())
            && altSlot.GetQuantity() < altSlot.GetItem().GetMaxStack())).Count() == 0)
            return;

        ItemStack removedStack = DropSlot(slot);
        ItemStack remainingItems = InvManager.Get().GetAltInv(this).AddItem(removedStack);

        if (!remainingItems.IsEmpty())
            slot.SetItemStack(remainingItems);
    }

    public ItemStack AddItem(ItemStack itemStack)
    {
        foreach (Slot slot in slots)
        {
            if (slot.IsEmpty())
                continue;

            if (slot.GetItem().Equals(itemStack.GetItem()))
            {
                int quantityLeft = slot.GetItem().GetMaxStack() - slot.GetQuantity();
                int quantityToTransfer = Math.Min(quantityLeft, itemStack.GetQuantity());
                slot.ChangeQuantity(quantityToTransfer);
                itemStack.ChangeQuantity(-quantityToTransfer);
            }
        }

        if (itemStack.GetQuantity() > 0)
        {
            foreach (Slot slot in slots)
            {
                if (slot.IsEmpty())
                {
                    slot.SetItemStack(itemStack);
                    return new ItemStack(null, 0);
                }
            }
        }

        return itemStack;
    }

    public static ItemStack DropSlot(Slot slot)
    {
        if (slot.IsEmpty())
            return new ItemStack(null, 0);

        int quantity;
        if (Input.IsActionPressed("slotAll"))
        {
            quantity = slot.GetQuantity();
            slot.SetItemStack(new ItemStack(slot.GetItem(), 0));
        }
        else
        {
            quantity = 1;
            slot.ChangeQuantity(-1);
        }
        
        return new ItemStack(slot.GetItem(), quantity);
    }

}
