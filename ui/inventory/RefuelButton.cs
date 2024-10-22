using Godot;
using System;
using System.Collections.Generic;

public partial class RefuelButton : Button
{
	[Export]
	Inventory campfireInv;

	private CustomSignals CS;

    public override void _Ready()
    {
        base._Ready();
        CS = GetNode<CustomSignals>("/root/CustomSignals");
    }

    public void OnButtonDown()
    {
        Slot campfireSlot = campfireInv.GetSlots()[0];
        ItemStack itemStack = new ItemStack(campfireSlot.GetItem(), campfireSlot.GetQuantity());
        if (!itemStack.IsEmpty() && itemStack.GetItem() is Fuel && campfireInv.UseItems(new List<ItemStack>() { campfireSlot.GetItemStack() }))
        {
            CS.EmitSignal(CustomSignals.SignalName.ReFuelFire, itemStack.GetQuantity() * ((Fuel) itemStack.GetItem()).GetFuelValue());
        }
    }
}
