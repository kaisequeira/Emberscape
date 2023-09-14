using Godot;
using System;

public partial class CustomSignals : Node
{

    #region Spawn Signals

    [Signal]
    public delegate void SpawnItemEventHandler(Slot slot, Vector2 position, Vector2 velocity);

    #endregion

}