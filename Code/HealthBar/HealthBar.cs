using Godot;
using System;

public partial class HealthBar : Node2D
{
    TextureProgressBar progressBar;
    Control helperNode;

    public override void _Ready()
    {
        helperNode = (Control)GetChild(0);
        progressBar = (TextureProgressBar)helperNode.GetChild(0);

    }

    public void Connect(Node2D parentNode, Vector2 offset, float size)
    {
        parentNode.GetParent().AddChild(this);

        var remoteTransform = new RemoteTransform2D();
        remoteTransform.UpdateRotation = false;
        remoteTransform.RemotePath = GetPath();
        parentNode.AddChild(remoteTransform);

        helperNode.Position = offset;
        helperNode.Scale = Vector2.One * size;
    }

    public void SetValue(int value)
    {
        progressBar.Value = value;
    }
}
