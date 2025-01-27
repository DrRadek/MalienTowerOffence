using Godot;
using System;

public partial class UI : Node
{
    [Export] Player player;
    [Export] Label coordsInfo;
    [Export] Label alienStuffInfo;
    [Export] Label baseHealthInfo;

    public override void _Ready()
    {
        player.OnPositionChanged += Player_OnPositionChanged;
        Inventory.Instance.OnAlientStuffAmountChanged += Instance_OnAlientStuffAmountChanged;
    }

    private void Instance_OnAlientStuffAmountChanged(int alienStuffAmount)
    {
        alienStuffInfo.Text = alienStuffAmount.ToString();
    }

    private void Player_OnPositionChanged(Vector2 pos)
    {
        coordsInfo.Text = $"{pos.X.ToString("F0")}, {pos.Y.ToString("F0")}";
    }
}
