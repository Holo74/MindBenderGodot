using Godot;
using System;

public class UpgradePickupBase : Spatial
{
    [Export]
    private string upgradeName = "";
    public override void _Ready()
    {
        if (WorldManager.instance.WorldInfoHas(upgradeName))
        {
            if (WorldManager.instance.GetWorldInfoData<bool>(upgradeName))
            {
                QueueFree();
                return;
            }
        }
        GetChild<Area>(0).Connect("body_entered", this, nameof(UpgradeCollected));
    }

    private void UpgradeCollected(Node body)
    {
        if (body.Name.Equals("Player"))
        {
            if (PlayerController.Instance.upgrades.GetUpgrade(upgradeName))
            {
                QueueFree();
                return;
            }
            PlayerController.Instance.upgrades.SetUpgradeTo(upgradeName, true);
            WorldManager.instance.AddToWorldInfo(upgradeName, true);
            QueueFree();
        }
    }
}
