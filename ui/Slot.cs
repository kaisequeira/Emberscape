using Godot;
using System;

public partial class Slot : Panel
{
    private Inventory inventory;
    private ItemStack itemStack;

    private Label QuantityLabel;
    private TextureRect Texture;
    private TextureRect SelectTexture;

    private CustomSignals CS;

    public override void _Ready()
    {
		CS = GetNode<CustomSignals>("/root/CustomSignals");

        QuantityLabel = GetNode<Label>("Quantity");
        Texture = GetNode<TextureRect>("TextureRect");
        SelectTexture = GetNode<TextureRect>("Selected");
        inventory = GetParent().GetParent<Inventory>();
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent
            && mouseEvent.ButtonIndex == MouseButton.Left
            && mouseEvent.Pressed)
        {
            if (InvManager.Get().GetAltInv(inventory) != null)
                inventory.TransferSlot(this);
        }
    }

    public void SetItemStack(ItemStack itemStack)
    {
        this.itemStack = itemStack;
        UpdateLabel();
    }

    public Item GetItem()
    {
        return itemStack.GetItem();
    }

    public int GetQuantity()
    {
        return itemStack.GetQuantity();
    }

    public void ChangeQuantity(int change)
    {
        itemStack.ChangeQuantity(change);
        UpdateLabel();
    }

    public bool IsEmpty()
    {
        return itemStack.IsEmpty();
    }

    public void SetSelected(bool selected)
    {
        SelectTexture.Visible = selected;
    }

    public void UpdateLabel()
    {
        if (!itemStack.IsEmpty())
        {
            Texture.Texture = GD.Load(itemStack.GetItem().GetSpritePath()) as Texture2D; 
            if (itemStack.GetItem().CanStack())
            {
                QuantityLabel.Show();
                QuantityLabel.Text = itemStack.GetQuantity().ToString();
            }
            else
            {
                QuantityLabel.Hide();
            }
        }
        else
        {
            Texture.Texture = GD.Load("res://art/items/empty.png") as Texture2D;
            QuantityLabel.Hide();
        }
    }

}
