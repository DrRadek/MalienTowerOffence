using Godot;
using System;

public partial class Player : RigidBody2D, IHitable
{
    [Export] Node2D weapon;
    [Export] Node2D repair;
    [Export] Area2D pickArea;
    [Export] PackedScene healthBarScene;

    [Export] float speed = 5000;
    [Export] float steeringAngle = 20;

    [Export] MouseDetector mouseDetector;

    IUsable weaponInstance;
    IUsable repairInstance;
    HealthBar healthBar;


    bool weaponEnabled = false;
    bool repairingEnabled = false;

    int maxHealth = 6;
    int health; // = 3;

    public int Health
    {
        get => health;
        set
        {
            health = value;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            healthBar.SetValue(health * 100 / maxHealth);
            //healthBar.Visible = health != maxHealth;
        }
    }


    public bool WeaponEnabled { private get => weaponEnabled; set => weaponEnabled = value; }
    public bool RepairingEnabled { private get => repairingEnabled; set => repairingEnabled = value; }

    // events
    public delegate void onPositionChanged(Vector2 pos);
    public event onPositionChanged OnPositionChanged;

    private void Instance_OnRepair(int amount)
    {
        if (mouseDetector.IsMouseDetected())
        {
            Health += amount;
        }
    }

    public override async void _Ready()
    {
        GameManager.Instance.OnRepair += Instance_OnRepair;

        weaponInstance = (IUsable)weapon;
        repairInstance = (IUsable)repair;
        pickArea.AreaEntered += CollectibleArea_AreaEntered;
        // BodyEntered += OnBodyEntered;


        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        healthBar = (HealthBar)healthBarScene.Instantiate();
        healthBar.Connect(this, new Vector2(0, -50), 1);
        Health = maxHealth;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed(GameInput.Shoot))
        {
            if (weaponEnabled)
                weaponInstance.Use();
            else if (RepairingEnabled)
            {
                if (repairInstance.Use())
                {
                    GameManager.Instance.InvokeRepair();
                }
            }
        } 

        float forward = (Input.IsActionPressed(GameInput.MovementBackward) ? 1 : 0) - (Input.IsActionPressed(GameInput.MovementForward) ? 1 : 0);
        float right = (Input.IsActionPressed(GameInput.MovementRight) ? 1 : 0) - (Input.IsActionPressed(GameInput.MovementLeft) ? 1 : 0);

        var directionVector = new Vector2(right, forward).Normalized();
        //ApplyCentralForce(directionVector * speed); // Transform.X * 

        // Movement V1
        ApplyForce(GlobalTransform.X * (forward != 0 || right != 0 ? 1 : 0) * speed); // , GlobalTransform.Y * directionVector

        // Movement V2
        //ApplyForce(-GlobalTransform.X * forward * speed);

        var mouseDirection = GetGlobalMousePosition() - weapon.GlobalPosition;

        // Rotate weapon
        var lookAtVector = weapon.GlobalPosition + mouseDirection.Normalized();
        weapon.Rotation = Mathf.LerpAngle(weapon.Rotation, weapon.Rotation + weapon.GetAngleTo(lookAtVector), 0.1f);
        //rotatedNode.LookAt(Position + mouseDirection.Normalized());

        //directionVector = Vector2.Right;
        // Rotate rover

        // Movement V1
        var newRotation = Mathf.LerpAngle(Rotation, Rotation + GetAngleTo(GlobalPosition + directionVector), 0.1f);
        var difference = Math.Clamp(newRotation - Rotation, -0.08f, 0.08f);
        Rotation += difference;
        // Movement V2
        //Rotate(0.08f * right);


        //if (Rotation - newRotation < 0.1)
        //    Rotate(0.05f);
        //else
        //    Rotate(-0.05f);
        //Rotate(Rotation - newRotation);

    }

    public override void _Process(double delta)
    {
        OnPositionChanged?.Invoke(Position);
    }



    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        LinearVelocity *= 0.9f;
        AngularVelocity *= 0.5f;
    }

    private void CollectibleArea_AreaEntered(Area2D area)
    {
        if (area is not ICollectible collectible)
            return;

        collectible.Collect();
    }

    public void GetHit(int damage)
    {
        Health -= damage;

        if(Health <= 0)
        {
            Health = maxHealth;
            Position = Vector2.Zero;
        }
    }
}
