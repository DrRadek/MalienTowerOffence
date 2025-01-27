using Godot;
using System;

public partial class FightAction : Node, IPlayerAction
{
    [Export] Player player;
    [Export] Resource cursor;
    public void Start()
    {
        Input.SetCustomMouseCursor(cursor);
        player.WeaponEnabled = true;
    }

    public void Stop()
    {
        player.WeaponEnabled = false;
    }
}
