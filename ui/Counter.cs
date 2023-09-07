using Godot;
using System;

public partial class Counter : HBoxContainer
{
	private Label Number;
	private AnimationPlayer Animator;
	private bool isEnabled = true;

	public override void _Ready()
	{
		Number = GetNode<Label>("Number");
		Animator = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public void SetCounter(int dayNum)
	{
		Number.Text = $"{dayNum}";
	}

	public void ToggleCounterOn()
	{
		Animator.Play("fadeIn");
	}

	public void ToggleCounterOff()
	{
		Animator.Play("fadeOut");
	}

}