using Godot;

public partial class DayManager : Node
{
    [Export]
    private SubViewport Viewport;

    private PackedScene DayScene;
    private Day day;

    private PackedScene UiScene;
    private UI ui;

    private int dayNum = 3;
    private AnimationPlayer animPlayer;
    private CustomSignals CS;

    public override void _Ready()
    {
        DayScene = GD.Load<PackedScene>("res://days/Day.tscn");
        UiScene = GD.Load<PackedScene>("res://ui/UI.tscn");
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        CS = GetNode<CustomSignals>("/root/CustomSignals");
        CS.Connect(CustomSignals.SignalName.DayEnded, new Callable(this, nameof(TransitionToScene)));
        TransitionToScene();
    }

	public void TransitionToScene()
	{
        animPlayer.Play("FadeOut");
        
        if (day != null)
            day.QueueFree();
        if (ui != null)
            ui.QueueFree();
        
        ui = UiScene.Instantiate() as UI;
        AddChild(ui);

        day = DayScene.Instantiate() as Day;
        Viewport.AddChild(day);

        day.Initialise(100, 120);

        animPlayer.Play("FadeIn");
	}

    public int GetDayCount => dayNum;
}