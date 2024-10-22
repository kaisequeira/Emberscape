using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class UI : CanvasLayer
{
    public static UI Instance;
	public Control RuntimeContainer;
	private ColorRect Dimmer;
	
	private List<Inventory> inventories = new List<Inventory>();
    private Inventory AlternativeInventory;

	public enum IndicatorType
	{
		Fire,
		Rain,
		Wind,
		Food,
	}

	[Export]
	private VBoxContainer IndicatorContainer;

	[Export]
	private NightTracker TimeLabel;

	[Export]
	private Control CraftingStumpContainer;

	[Export]
	private Control CraftingStoolContainer;

    public override void _EnterTree()
    {
        base._EnterTree();

       if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            QueueFree();
        }
    }

    public override void _Ready()
	{
		RuntimeContainer = GetNode<Control>("Runtime");
		Dimmer = GetNode<ColorRect>("Dimmer");
		FindInventories(this);
		TimeLabel.SetCounter((GetParent() as DayManager).GetDayCount);
	}

	public void SetTimeLabel(float percentage)
	{
		TimeLabel.UpdateTime(percentage);
	}

	private void FindInventories(Node curNode)
	{
		foreach (Node node in curNode.GetChildren())
		{
			if (node is Inventory)
				inventories.Add(node as Inventory);
			else
				FindInventories(node);
		}
	}

    public static UI Get()
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

	public ButtonUI InstanceButton(Vector2 position, string text)
	{
		ButtonUI button = (ResourceLoader.Load("res://ui/ButtonUI.tscn") as PackedScene)
						  .Instantiate<ButtonUI>();
		RuntimeContainer.AddChild(button);
		button.SetGlobalPosition(button.GlobalPosition + position * 10);
		button.SetText(text);
		return button;
	}

	public void ChangeScreenOpacity(float opacity)
	{
		Dimmer.Modulate = new Color(0, 0, 0, opacity);
	}

	public Indicator GetIndicator(IndicatorType indicatorType)
	{
		Indicator indicator = IndicatorContainer.GetNodeOrNull(indicatorType + "Indicator") as Indicator;

    	if (indicator == null)
		{
			string indicatorPath = $"res://ui/indicators/{indicatorType}Indicator.tscn";
			indicator = (ResourceLoader.Load(indicatorPath) as PackedScene).Instantiate<Indicator>();
			IndicatorContainer.AddChild(indicator);
		}

    	return indicator;
	}

	public Control GetStoolCrafting()
	{
		return CraftingStoolContainer;
	}

	public Control GetStumpCrafting()
	{
		return CraftingStumpContainer;
	}

}
