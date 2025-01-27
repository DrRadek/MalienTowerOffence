using Godot;
using System;

public partial class AlienStuff : Area2D, ICollectible
{
    int alientStufftAmount;

    public int AlientStufftAmount { get => alientStufftAmount; set => alientStufftAmount = value; }

    public void Collect()
    {
        Inventory.Instance.AlientStuffAmount += alientStufftAmount;
        QueueFree();
    }
}
