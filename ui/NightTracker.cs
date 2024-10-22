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
	
	public void UpdateTime(float percentage)
	{
		int totalMinutes = 6 * 60;

		int minutes = (int) (totalMinutes * percentage % 60);
		int hours = (int) (totalMinutes * percentage / 60);

		if (hours == 0)
			hours = 12;

		string formattedTime = string.Format("{0:D2}:{1:D2}am", hours, minutes);

		TimeLabel.Text = formattedTime;
	}
}
