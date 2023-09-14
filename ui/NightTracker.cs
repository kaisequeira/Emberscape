using Godot;
using System;

public partial class NightTracker : VBoxContainer
{
	private HBoxContainer DayCounterContainer;
	private Label Number;
	private Label TimeLabel;

	public override void _Ready()
	{
		DayCounterContainer = GetNode<HBoxContainer>("HBoxContainer");
		TimeLabel = GetNode<Label>("Time");
		Number = DayCounterContainer.GetNode<Label>("Number");
	}

	public void SetCounter(int dayNum)
	{
		Number.Text = $"{dayNum}";
	}
	
	public void UpdateTime(string time)
	{
		TimeLabel.Text = time;
	}

}
