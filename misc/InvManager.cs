using System;
using System.Collections.Generic;
using Godot;

public partial class InvManager : Node
{
    public static InvManager Instance;

    private List<Inventory> inventories;
    private Inventory AlternativeInventory;

    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            QueueFree();
        }
    }

    public static InvManager Get()
    {
        return Instance;
    }

    public void SetInventories(List<Inventory> inventories)
    {
        this.inventories = inventories;
    }

    public void SetAltInv(Inventory inventory)
    {
        AlternativeInventory = inventory;
    }

	public Inventory GetInventory(Inventory.Types type)
	{
		foreach (Inventory inventory in inventories)
		{
			if (inventory.GetInvType() == type)
			{
				return inventory;
			}
		}

		return null;
	}

    public Inventory GetAltInv(Inventory curInventory)
    {
        if (curInventory != null && curInventory.GetInvType() != Inventory.Types.Player)
        {
            return GetInventory(Inventory.Types.Player);
        }

        return AlternativeInventory;
    }

}
