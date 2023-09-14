using Godot;

public partial class DayManager : Node
{
    [Export]
    private SubViewport Viewport;
    private PackedScene UIScene;
    private UI ui;

    private PackedScene DayScene;
    private Day day;    
    static private int dayCounter;

    public override void _Ready()
    {
        DayScene = GD.Load<PackedScene>("res://days/Day.tscn");
        UIScene = GD.Load<PackedScene>("res://ui/UI.tscn");
        LoadDay(0);
    }

    private void LoadDay(int offset)
    {
        dayCounter += offset;
        
        ui = UIScene.Instantiate<UI>();
        AddChild(ui);
        // Initialise UI

        day = DayScene.Instantiate<Day>();
        Viewport.AddChild(day);
        day.Initialise(dayCounter);
    }

    public void EndDay()
    {
        day.QueueFree();
        ui.QueueFree();
        LoadDay(1);
    }
}