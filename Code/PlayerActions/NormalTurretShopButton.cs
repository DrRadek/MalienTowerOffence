using Godot;
using System;

public partial class NormalTurretShopButton : ShopButton
{

    public override Node2D PrepareAsset()
    {
        var instance = sceneToBuy.Instantiate();
        instance.ProcessMode = ProcessModeEnum.Disabled;
        GameManager.Instance.storageNode.AddChild(instance);
        
        return (Node2D)instance;
    }
}
