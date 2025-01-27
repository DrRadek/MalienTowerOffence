using Godot;
using System;
using System.Text;

public partial class Base : StaticBody2D, IHitable
{
    [Export] PackedScene healthBarScene;
    [Export] MouseDetector mouseDetector;

    [Export] HealthBar uiHealthBar;
    HealthBar healthBar;

    int maxHealth = 100;
    int health;

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
            var healthPercent = health * 100 / maxHealth;
            healthBar.SetValue(healthPercent);
            uiHealthBar.SetValue(healthPercent);
            //healthBar.Visible = health != maxHealth;
        }
    }

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

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        healthBar = (HealthBar)healthBarScene.Instantiate();
        healthBar.Connect(this, new Vector2(0, -120), 1.5f);

        Health = maxHealth;

    }

    public void GetHit(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            GameStatus.Instance.SetLoseStatus();
        }
    }
}
