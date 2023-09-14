using Godot;
using System;
using System.Collections.Generic;

public partial class UI : CanvasLayer
{	
	private List<Inventory> inventories = new List<Inventory>();
	private Control inventoryContainer;

	public override void _Ready()
	{
		inventoryContainer = GetNode<VBoxContainer>("InventoryContainer");

		foreach (Inventory inventory in inventoryContainer.GetChildren())
		{
			inventories.Add(inventory);
		}

		InvManager.Get().SetInventories(inventories);
	}
}
