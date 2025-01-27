using Godot;
using System;
using System.Collections.Generic;

public partial class Enemy : RigidBody2D, IHitable
{
    public delegate void OnDeathDelegate();
    public OnDeathDelegate OnDeath;

    [Export] PackedScene alienStuffScene;
    [Export] Node2D target;
    [Export] Area2D enemyFinder;
    [Export] float speed = 40f;
    [Export] int alienStuffDropAmount = 1;
    //[Export] Node weapon;
    //IUsable weaponInstance;
    [Export] PackedScene healthBarScene;

    HealthBar healthBar;

    int maxHealth = 1;
    int health; // = 3;
    int damage = 1;

    public int Health { 
        get {
            return health;
        }
        set {
            health = value;
            healthBar.SetValue(health * 100 / maxHealth);
            healthBar.Visible = health != maxHealth;
        } 
    }

    public int Damage { 
        get {
            return damage;
        } 
        set => damage = value;
    }

    public override void _Ready()
    {
        damage = (int)(damage * (1 + (GameManager.Instance.DifficultyScale * 0.25)));
        maxHealth = (int)(maxHealth * (1.2 + (GameManager.Instance.DifficultyScale * 0.4)));
        alienStuffDropAmount = (int)(alienStuffDropAmount * (1.2 + (GameManager.Instance.DifficultyScale * 0.1)));

        target = (Node2D)GetParent().GetParent().GetParent().GetParent().GetNode("Base");

        BodyEntered += EnemyBodyEntered;

        healthBar = (HealthBar)healthBarScene.Instantiate();
        healthBar.Connect(this, new Vector2(0,-50), 1);

        Health = maxHealth;
        //weaponInstance = (IUsable)weapon;
    }

    private void EnemyBodyEntered(Node body)
    {
        //if (body is not IShieldCollectible shieldCollictible || shieldCollictible.IsFiredFromShield() == false || body.IsQueuedForDeletion())
        //    return;

        //body.QueueFree();

        //health--;
        //if(health == 0)
        //{
        //    OnDeath?.Invoke();
        //    QueueFree();
        //}
    }

    double hitCooldown = 0;
    double hitRate = 0.5f;

    public override void _PhysicsProcess(double delta)
    {
        //weaponInstance.Use();

        var direction = (target.GlobalPosition - GlobalPosition).Normalized();

        var lookAtVector = Position + direction;
        Rotation = Mathf.LerpAngle(Rotation, Rotation + GetAngleTo(lookAtVector), 0.1f);

        ApplyCentralForce(direction * speed);

        //ApplyCentralForce((playerTarget.GlobalPosition - GlobalPosition) * 2);
        //LookAt(playerTarget.GlobalPosition);

        if(hitCooldown <= 0)
        {
            hitCooldown = 0;
            HitNearbyEnemies();
        }
        else
        {
            hitCooldown -= delta;
        }
    }

    private void HitNearbyEnemies()
    {
        var areas = enemyFinder.GetOverlappingAreas();
        bool hitSomeone = false;
        foreach (var area in areas)
        {
            if (area.GetParent() is not IHitable hitable)
                continue;

            hitable.GetHit(Damage);
            hitSomeone = true;
        }

        if (hitSomeone)
        {
            hitCooldown = hitRate;
        }
    }

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        if (LinearVelocity.Length() > 500)
            LinearVelocity *= 0.5f;
    }

    public void GetHit(int damage)
    {
        Health -= damage;
        if(Health <= 0)
        {
            DropAlienStuff();
            DeleteObject();

            // TODO: on death (drop loot)
        }
    }

    void DropAlienStuff()
    {
        var instance = (AlienStuff)alienStuffScene.Instantiate();
        instance.AlientStufftAmount = alienStuffDropAmount;
        //AddChild(instance);
        CallDeferred(MethodName.AddChild, instance);
        //GameManager.Instance.storageNode.CallDeferred(MethodName.AddChild, instance);

        //GameManager.Instance.storageNode.AddChild(instance);
        //instance.Reparent(GameManager.Instance.storageNode);
        instance.CallDeferred(MethodName.Reparent, GameManager.Instance.storageNode);
    }

    void DeleteObject()
    {
        healthBar.QueueFree();
        QueueFree();
    }
}
