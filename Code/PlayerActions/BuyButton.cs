using Godot;
using System;

public partial class BuyButton : Button
{
    [Export] Label infoLabel;
    [Export] Label nameLabel;
    [Export] Control infoNode;
    [Export] BuildAction buildAction;

    ShopButton activeButton = null;

    public override void _Ready()
    {
        ButtonDown += BuyButton_ButtonDown;
    }

    private void BuyButton_ButtonDown()
    {
        if (activeButton == null)
            return;

        if (!activeButton.isResearched)
        {
            if (Inventory.Instance.AlientStuffAmount >= activeButton.researchPrice)
            {
                Inventory.Instance.AlientStuffAmount -= activeButton.researchPrice;
                activeButton.isResearched = true;
                UpdateInfo(activeButton);
            }
        }
        else
        {
            if (Inventory.Instance.AlientStuffAmount >= activeButton.price)
            {
                //Inventory.Instance.AlientStuffAmount -= activeButton.price;
                buildAction.StartBuilding(activeButton.PrepareAsset(),activeButton.sceneToBuy,  activeButton.price);
            }
        }
    }

    public void UpdateInfo(ShopButton button)
    {
        infoNode.Visible = true;
        activeButton = button;
        nameLabel.Text = button.Text;

        if (!button.isResearched)
        {
            Text = $"Research (Price: {button.researchPrice})";
        }
        else
        {
            Text = $"Build (Price: {button.price})";
        }

        infoLabel.Text = button.infoText;
    }
}
