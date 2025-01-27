using Godot;
using System;

public partial class Spawner : Node2D, IHitable
{
    [Export] PackedScene enemyScene;
    [Export] PackedScene healthBarScene;
    [Export] bool debugDisable = false;

    HealthBar healthBar;

    MapArea area;
    
    bool isActive = false;

    [Export] int maxHealth = 300;
    int health; // = 3;

    public int Health
    {
        get => health;
        set
        {
            health = value;
            healthBar.SetValue(health * 100 / maxHealth);
            //healthBar.Visible = health != maxHealth;
        }
    }

    public override async void _Ready()
    {
        area = (MapArea)GetParent();
        area.AliveSpawnersCount++;
        area.OnActiveChanged += Area_OnActiveChanged;

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        healthBar = (HealthBar)healthBarScene.Instantiate();
        healthBar.Connect(this, new Vector2(0, -70), 1.3f);
        Health = maxHealth;
    }

    double GetSpawnSpeed()
    {
        return GameManager.Instance.gameStarted ? Mathf.Max(0.25, 7.0 - GameManager.Instance.DifficultyScale * 0.05) : 0.1f;
    }

    async void SpawnLogic()
    {
        while (true)
        {
            await ToSignal(GetTree().CreateTimer(GetSpawnSpeed()), Timer.SignalName.Timeout);

            if (!isActive)
                return;

            if (GameManager.Instance.gameStarted) { 
                var enemy = enemyScene.Instantiate();
                AddChild(enemy);
            }
        }

    }

    private void Area_OnActiveChanged(bool active)
    {
        if (debugDisable)
            return;

        //enemy.Instantiate
        bool wasActiveAlready = isActive;
        isActive = active;

        if(isActive && !wasActiveAlready)
            SpawnLogic();
    }

    public void GetHit(int damage)
    {
        if(Health <= 0) return;
        Health -= damage;
        if(Health <= 0)
        {
            area.AliveSpawnersCount--;
            healthBar.QueueFree();
            QueueFree();
        }
    }
}
