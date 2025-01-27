using Godot;
using System;

public partial class RepairAction : Node, IPlayerAction
{
    [Export] Player player;
    [Export] Resource cursor;
    public void Start()
    {
        Input.SetCustomMouseCursor(cursor);
        player.RepairingEnabled = true;
    }

    public void Stop()
    {
        player.RepairingEnabled = false;
    }
}
