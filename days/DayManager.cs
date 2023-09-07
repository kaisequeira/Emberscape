using Godot;

public partial class DayManager : Node
{
    private PackedScene DayScene;
    private Control UI;
    private Counter Counter;
    private Day currentDay;
    
    [Export]
    private SubViewport Viewport;

    static private int dayCounter;

    public override void _Ready()
    {
        UI = GetNode<Control>("Control");
        Counter = UI.GetNode<Counter>("Counter");
        DayScene = GD.Load<PackedScene>("res://days/Day.tscn");
        LoadDay(false);
    }

    private void DeleteDay()
    {
        currentDay.QueueFree();
    }

    private void LoadDay(bool loadNext)
    {
        if (loadNext) dayCounter++;
        currentDay = DayScene.Instantiate<Day>();
        currentDay.Initialise(dayCounter);
        Viewport.AddChild(currentDay);
    }

    public void EndCurrentDay()
    {
        DeleteDay();
        LoadDay(true);
    }

    public void UpdateUI()
    {
        Counter.SetCounter(currentDay.GetDayCount());
    }

}