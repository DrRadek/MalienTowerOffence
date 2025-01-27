using Godot;
using System;

public partial class Bullet : RigidBody2D //, IShieldCollectible
{
    //[Export] CanvasItem canvasItem;

    // if it can hit it, it will damage it
    int damage = 1;

    bool isFiredFromShield = false;

    double maxLifetime = 2f;
    Timer timer;

    // Set damage by weapon
    public int Damage { set => damage = value; }

    public override void _Ready()
    {
        BodyEntered += EnemyBodyEntered;

        timer = new Timer();
        AddChild(timer);
        timer.Start(maxLifetime);
        timer.Timeout += TimerTimeout;

        LinearVelocity = GlobalTransform.X * 800;
    }

    private void EnemyBodyEntered(Node body)
    {
        var hitableBody = body as IHitable;
        if (hitableBody == null)
            return;

        hitableBody.GetHit(damage);
        QueueFree();

    }

    private void TimerTimeout()
    {
        QueueFree();
    }
}
