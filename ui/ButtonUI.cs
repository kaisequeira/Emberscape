using Godot;
using System;

public partial class ButtonUI : Control
{
	private Label Unclicked;
	private Label Clicked;

	public override void _Ready()
	{
		Unclicked = GetNode<Label>("Unclicked");
		Clicked = GetNode<Label>("Clicked");
	}

    public void SetText(string text)
	{
		if (Unclicked == null || Clicked == null || !IsInstanceValid(Unclicked) || !IsInstanceValid(Clicked))
			return;

		Unclicked.Text = "  " + text + "  ";
		Clicked.Text = "  " + text + "  ";
	}

	public void SetClicked(bool clicked)
	{
		if (Unclicked == null || Clicked == null || !IsInstanceValid(Unclicked) || !IsInstanceValid(Clicked))
			return;

		Clicked.Visible = clicked;
		Unclicked.Visible = !clicked;
	}
}
