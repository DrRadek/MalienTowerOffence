using Godot;
using System;

public partial class Inventory : Node
{
    private int alientStuffAmount = 0;

    public static Inventory Instance { get; private set; }
    public int AlientStuffAmount { 
        get => alientStuffAmount; 
        set { 
            alientStuffAmount = value; 
            OnAlientStuffAmountChanged?.Invoke(AlientStuffAmount);
        }
    }

    // events
    public delegate void onAlientStuffAmountChanged(int alienStuffAmount);
    public event onAlientStuffAmountChanged OnAlientStuffAmountChanged;



    public override async void _Ready()
    {
        Instance = this;
        //AlientStuffAmount = 0;
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        AlientStuffAmount = 0;
    }
}
