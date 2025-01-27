using Godot;
using System;

public partial class DeleteAction : Node, IPlayerAction
{
    //[Export] Player player;
    [Export] Resource cursor;
    public void Start()
    {
        Input.SetCustomMouseCursor(cursor);
        GameManager.Instance.RemoveEnabled = true;
    }

    public void Stop()
    {
        GameManager.Instance.RemoveEnabled = false;
    }
}
