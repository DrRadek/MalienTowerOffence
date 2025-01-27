using Godot;
using System;

public partial class MapArea : Node
{
    bool isActive = true;
    public delegate void onActiveChanged(bool active);
    public event onActiveChanged OnActiveChanged;

    private int aliveSpawnersCount = 0;

    public int AliveSpawnersCount { get => aliveSpawnersCount; 
        set {
            aliveSpawnersCount = value;
            if (aliveSpawnersCount == 0)
            {
                GameStatus.Instance.SetWinStatus();
            }

        }
    }

    public override void _Ready()
    {
        OnActiveChanged?.Invoke(isActive);
    }
}
