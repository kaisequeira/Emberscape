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
        inventory = GetParent().GetParentOrNull<Inventory>();
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent
            && mouseEvent.ButtonIndex == MouseButton.Left
            && mouseEvent.Pressed)
        {
            if (UI.Get().GetAltInv(inventory) != null)
                inventory.TransferSlot(this);
        }
    }

    public void SetItemStack(ItemStack itemStack)
    {
        this.itemStack = itemStack;
        UpdateLabel();
    }

    public ItemStack GetItemStack()
    {
        return itemStack;
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

    public void SetSlotSize(float size)
    {
        CustomMinimumSize = new Vector2(
            CustomMinimumSize.X * size,
            CustomMinimumSize.Y * size
        );
        
        QuantityLabel.Scale *= size;
    }

    public void ExpandSlot(float numExpansions)
    {
        Vector2 originalSize = CustomMinimumSize;
        CustomMinimumSize *= new Vector2(numExpansions, 1);
        CustomMinimumSize += new Vector2(numExpansions * 5, 0);
        QuantityLabel.Position -= new Vector2(
            Mathf.Max(CustomMinimumSize.X - originalSize.X, 0) / 2, 0
        );
    }

}
