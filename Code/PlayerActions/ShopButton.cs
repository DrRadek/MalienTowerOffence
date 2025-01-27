using Godot;
using System;

public partial class ShopButton : Button
{
    [Export] public PackedScene sceneToBuy;
    [Export(PropertyHint.MultilineText)] public string infoText;
    [Export] public int price;
    [Export] public int researchPrice;
    [Export] BuyButton buyButton;

    public bool isResearched = false;

    public override void _Ready()
    {
        if (researchPrice == 0)
        {
            isResearched = true;
        }

        ButtonDown += ShopButton_ButtonDown;

        infoText = infoText.Replace("$PRICE$", price.ToString());
    }

    private void ShopButton_ButtonDown()
    {
        buyButton.UpdateInfo(this);
    }

    public virtual Node2D PrepareAsset() { 
        throw new NotImplementedException();
    }

    //public void ReturnInfo()
    //{

    //}
}
