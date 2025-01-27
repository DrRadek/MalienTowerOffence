using Godot;
using System;

public partial class Turret : StaticBody2D, IHitable
{
    [Export] Node2D weapon;
    [Export] CollisionShape2D enemyFinderRadius;
    [Export] Area2D enemyFilder; // TODO: implement getting a list of enemies and finding the closest one
    [Export] float range = 700;
    [Export] Sprite2D debugSprite;
    [Export] MouseDetector mouseDetector;
    [Export] public MeshInstance2D previewRangeMesh;

    IUsable weaponInstance;

    double enemySearchFrequency = 0.25f;
    double currentEnemySearch = 0;

    Node2D target;
    RigidBody2D targetRb;
    bool targetHasRb = false;

    [Export] PackedScene healthBarScene;

    HealthBar healthBar;

    int maxHealth = 30;
    int health;

    public int Health
    {
        get => health;
        set
        {
            health = value;
            if(health > maxHealth)
            {
                health = maxHealth;
            }
            healthBar.SetValue(health * 100 / maxHealth);
            //healthBar.Visible = health != maxHealth;
        }
    }

    public override async void _Ready()
    {
        weaponInstance = (IUsable)weapon;

        ((CircleShape2D)enemyFinderRadius.Shape).Radius = range;
        var mesh = (SphereMesh)previewRangeMesh.Mesh;
        mesh.Radius = range;
        mesh.Height = range * 2;

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        healthBar = (HealthBar)healthBarScene.Instantiate();
        healthBar.Connect(this, new Vector2(0, -70), 1.3f);

        Health = maxHealth;

        GameManager.Instance.OnRepair += Instance_OnRepair;
        GameManager.Instance.OnRemove += Instance_OnRemove;
    }

    private void Instance_OnRemove()
    {
        if (mouseDetector.IsMouseDetected())
        {
            QueueFree();
        }
    }

    private void Instance_OnRepair(int amount)
    {
        if (mouseDetector.IsMouseDetected())
        {
            Health += amount;
        }
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) { 
            healthBar.QueueFree();
            GameManager.Instance.OnRepair -= Instance_OnRepair;
            GameManager.Instance.OnRemove -= Instance_OnRemove;
            base.Dispose(disposing);
            
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        // Rotate weapon
        var lookAtVector = weapon.GlobalPosition + Vector2.Right;// rotatedNode.GlobalPosition + mouseDirection.Normalized();
        
        currentEnemySearch += delta;
        if(currentEnemySearch > enemySearchFrequency)
        {
            currentEnemySearch = 0;
            var areas = enemyFilder.GetOverlappingAreas();

            Area2D area = null;
            float minDistance = float.MaxValue;
            foreach (var area2 in areas) {
                float distance = (area2.GlobalPosition).DistanceTo(GlobalPosition);
                
                if (distance < minDistance) { 
                    minDistance = distance;
                    area = area2;
                    
                }
            }

            if (area != null) {
                debugSprite.GlobalPosition = area.GlobalPosition;
                target = (Node2D)area.GetParent();
                if (target is RigidBody2D rb) { 
                    targetRb = rb;
                    targetHasRb = true;
                }
                else
                {
                    targetHasRb = false;
                }
            }
            else
            {
                target = null;

            }
        }

        try { 
            if (target != null)
            {
                lookAtVector = weapon.GlobalPosition + (target.GlobalPosition + (targetHasRb ? targetRb.LinearVelocity : Vector2.Zero) - weapon.GlobalPosition).Normalized();
                weaponInstance.Use();
            }
        }
        catch {
            target = null;
        }

        weapon.Rotation = Mathf.LerpAngle(weapon.Rotation, weapon.Rotation + weapon.GetAngleTo(lookAtVector), 0.1f);

    }

    public void GetHit(int damage)
    {
        Health -= damage;
        if(Health <= 0)
        {
            //healthBar.QueueFree();
            QueueFree();
        }
    }

    //public void Disable()
    //{
    //    ProcessMode = ProcessModeEnum.Disabled;
    //}

    //public void Enable()
    //{
    //    throw new NotImplementedException();
    //}
}
