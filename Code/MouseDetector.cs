using Godot;
using System;

public partial class MouseDetector : Area2D
{
    public bool IsMouseDetected()
    {
        return GameManager.Instance.mouseArea.OverlapsArea(this);
    }
}
