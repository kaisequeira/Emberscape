using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Inventory : BoxContainer
{ 
    public static readonly int INVALID_SLOT = -1;

	public enum Types
	{
		Player,
		Crate,
		Logpile,
        Campfire,
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
        {
            Disable();
            foreach (Slot slot in slots)
			    slot.SetSlotSize(0.8f);                
        }
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
        UI.Get().SetAltInv(null);
        Visible = false;
    }

    public void Enable()
    {
        UI.Get().SetAltInv(this);
        Visible = true;
    }

    public void IncreaseSize(int newSlots)
    {
        for (int i = 0; i < newSlots; i++)
        {
            Slot slot = GD.Load<PackedScene>("res://ui/inventory/Slot.tscn").Instantiate<Slot>();
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
            int count = 0;
            foreach (Slot slot in slots)
            {
                if (slot.IsEmpty())
                    continue;

                if (slot.GetItem().Equals(itemStack.GetItem()))
                    count += Math.Min(slot.GetQuantity(), itemStack.GetQuantity());

                if (count >= itemStack.GetQuantity())
                    break;
            }
            
            if (count < itemStack.GetQuantity())
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

        if (UI.Get().GetAltInv(this).IsFull(slot.GetItem()))
            return;

        ItemStack removedStack = DropSlot(slot, true);
        ItemStack remainingItems = UI.Get().GetAltInv(this).AddItem(removedStack);

        if (!remainingItems.IsEmpty())
            slot.SetItemStack(remainingItems);
    }

    public ItemStack AddItem(ItemStack itemStack)
    {
        if (isIncompatible(itemStack.GetItem()))
            return itemStack;

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
                    int quantity = Mathf.Min(itemStack.GetQuantity(), itemStack.GetItem().GetMaxStack());
                    
                    itemStack.ChangeQuantity(-quantity);
                    slot.SetItemStack(new ItemStack(itemStack.GetItem(), quantity));
                }
                if (itemStack.IsEmpty())
                    return new ItemStack(null, 0);
            }
        }

        return itemStack;
    }

    public bool IsFull(Item item)
    {
        if (isIncompatible(item))
            return true;

        foreach (Slot slot in slots)
        {
            if (slot.IsEmpty())
                return false;
            else if (slot.GetItem().Equals(item) && slot.GetQuantity() < item.GetMaxStack())
                return false;
        }

        return true;
    }

    public static ItemStack DropSlot(Slot slot, bool allowShift)
    {
        if (slot.IsEmpty())
            return new ItemStack(null, 0);

        int quantity;
        if (allowShift && Input.IsActionPressed("slotAll"))
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

    private bool isIncompatible(Item item)
    {
        if (type is Types.Crate && item is Fuel)
            return true;
        else if (type is Types.Logpile && !(item is Fuel))
            return true;
        else if (type is Types.Campfire && !(item is Fuel))
            return true;
        else
            return false;
    }
}
