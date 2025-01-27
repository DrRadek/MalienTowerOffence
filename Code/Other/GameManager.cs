using Godot;
using System;

public partial class GameManager : Node2D
{
    [Export] public Node2D storageNode;
    [Export] public Area2D mouseArea;
    [Export] Label difficultyValueLabel;

    //public bool isRepairingEnabled = false;
    public delegate void onRepair(int amount);
    public event onRepair OnRepair;

    public delegate void onRemove();
    public event onRemove OnRemove;

    private Input.MouseModeEnum mouseMode1 = Input.MouseModeEnum.Visible;
    private Input.MouseModeEnum mouseMode2 = Input.MouseModeEnum.Captured;

    bool removeEnabled = false;
    public bool gameStarted = false;
    double difficultyScale = 1;
    double difficultyScalingSpeed = 1;

    public static GameManager Instance { get; private set; }
    public bool RemoveEnabled { get => removeEnabled; set => removeEnabled = value; }
    public double DifficultyScale { get => difficultyScale; set => difficultyScale = value; }
    public double DifficultyScalingSpeed { get => difficultyScalingSpeed; set => difficultyScalingSpeed = value; }

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEvent keyEvent && keyEvent.IsPressed())
        {
            if (@event.IsActionPressed(GameInput.Shoot))
            {
                if (removeEnabled)
                {
                    OnRemove?.Invoke();
                }
            }
            else if (@event.IsActionPressed(GameInput.MouseModeSwitch))
            {
                Input.MouseMode = Input.MouseMode == mouseMode1 ? mouseMode2 : mouseMode1;
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        mouseArea.GlobalPosition = GetGlobalMousePosition();
        difficultyScale += delta * difficultyScalingSpeed * 0.05; //* 0.02;
        difficultyValueLabel.Text = DifficultyScale.ToString("F2");
    }

    public void InvokeRepair()
    {
        OnRepair?.Invoke(2);
    }

    public override void _Ready()
    {
        Instance = this;
    }
}
