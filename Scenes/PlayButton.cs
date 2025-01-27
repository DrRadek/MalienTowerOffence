using Godot;
using System;

public partial class PlayButton : Button
{
    [Export] Node gameScene;
    [Export] Node menuScene;
    [Export] SpinBox spinBox;
    public override void _Ready()
    {
        ButtonDown += PlayButton_ButtonDown;
        spinBox.ValueChanged += SpinBox_ValueChanged;
    }

    double difficultyValue;
    private void SpinBox_ValueChanged(double value)
    {
        difficultyValue = value;
        GD.Print($"1: {difficultyValue}");
    }

    private async void PlayButton_ButtonDown()
    {
        gameScene.ProcessMode = ProcessModeEnum.Inherit;
        

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        GameManager.Instance.DifficultyScalingSpeed = spinBox.Value;
        GD.Print(GameManager.Instance.DifficultyScalingSpeed);
        GD.Print(spinBox.Value);
        GameManager.Instance.gameStarted = true;
        menuScene.QueueFree();
    }
}
