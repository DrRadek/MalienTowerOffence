using Godot;
using System;

public partial class Weapon : Node2D, IUsable
{
    [Export] Node storageNode;
    [Export] PackedScene ammoInstance;
    [Export] public int damage = 1;
    [Export] float bulletSize = 0.5f;

    bool isReady = true;
    [Export] public float cooldown = 0.3f;

    Timer timer = new Timer();

    public override void _Ready()
    {
        timer.OneShot = true;
        AddChild(timer);

        storageNode = GameManager.Instance.storageNode;
    }

    public bool Use()
    {
        if (timer.TimeLeft > 0)
            return false;

        if (ammoInstance != null)
        {
            var instance = (Bullet)ammoInstance.Instantiate();
            instance.Damage = damage;
            ((Sprite2D)instance.GetNode("Sprite2D")).Scale = Vector2.One * bulletSize;
            AddChild(instance);
            instance.Reparent(storageNode);
        }

        // Start cooldown
        timer.Start(cooldown);

        return true;
    }
}
