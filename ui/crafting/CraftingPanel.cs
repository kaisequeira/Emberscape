using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System;

public partial class CraftingPanel : HBoxContainer
{
	[Export]
	private Array<string> inputItems = new Array<string>();

	[Export]
	private Array<string> outputItems = new Array<string>();

	[Export]
	private Array<int> inputQuantity = new Array<int>();

	[Export]
	private Array<int> outputQuantity = new Array<int>();

	private List<ItemStack> inputStacks = new List<ItemStack>();
	private List<ItemStack> outputStacks = new List<ItemStack>();

	private string buttonPath;
	private CustomSignals CS;

	public override void _Ready()
	{
		CS = GetNode<CustomSignals>("/root/CustomSignals");
		PackedScene slotScene = (PackedScene) ResourceLoader.Load("res://ui/inventory/Slot.tscn");
		Slot slot;

		for (int i = 0; i < inputItems.Count; i++)
		{
			inputStacks.Add(
				new ItemStack(Activator.CreateInstance(Type.GetType(inputItems[i])) as Item, inputQuantity[i])
			);
			slot = slotScene.Instantiate<Slot>();
			AddChild(slot);
			slot.SetSlotSize(0.6f);

			if (inputItems.Count == 1)
				slot.ExpandSlot(2);

			slot.SetItemStack(inputStacks[i]);
		}

		AddChild((ResourceLoader.Load("res://ui/crafting/Arrow.tscn") as PackedScene).Instantiate<TextureRect>());

		for (int i = 0; i < outputItems.Count; i++)
		{
			outputStacks.Add(
				new ItemStack(Activator.CreateInstance(Type.GetType(outputItems[i])) as Item, outputQuantity[i])
			);
			slot = slotScene.Instantiate<Slot>();
			AddChild(slot);
			slot.SetSlotSize(0.6f);
			slot.SetItemStack(outputStacks[i]);
		}

		AddChild(((PackedScene) ResourceLoader.Load("res://ui/crafting/CraftingSpacer.tscn")).Instantiate<Control>());

		Button button = ((PackedScene) ResourceLoader.Load(buttonPath)).Instantiate<Button>();
		button.Connect(Button.SignalName.Pressed, new Callable(this, nameof(CraftItemStack)));
		AddChild(button);
	}

	public void CraftItemStack()
	{
		Inventory playerInv = UI.Get().GetInventory(Inventory.Types.Player);

		List<ItemStack> inputCopy = new List<ItemStack>();

		foreach (ItemStack stack in inputStacks)
		{
			inputCopy.Add(
				new ItemStack(Activator.CreateInstance(stack.GetItem().GetType()) as Item,
				stack.GetQuantity())
			);
		}
		
		ItemStack outputCopy;

		if (playerInv.UseItems(inputCopy))
			for (int i = 0; i < outputStacks.Count; i++)
			{
				outputCopy = new ItemStack(
					Activator.CreateInstance(outputStacks[i].GetItem().GetType()) as Item,
					outputStacks[i].GetQuantity()
				);

				outputCopy = playerInv.AddItem(outputCopy);

				if (!outputCopy.IsEmpty())
					CS.EmitSignal(CustomSignals.SignalName.SpawnItem, outputCopy, Vector2.Inf, Vector2.Inf);
			}
	}

	public void SetButtonPath(string path)
	{
		buttonPath = path;
	}

}
