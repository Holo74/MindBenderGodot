using System;
using Godot.Collections;

public class SavedData
{
    public static string SavedAreaPath = "SavedAreaPath";
    public SavedData()
    {
        upgrades = new PlayerUpgrade();
        SavedInPath.Add(SavedAreaPath, "res://Scenes/Start.tscn");
    }
    public SavedData(Dictionary playerUpgrades, Dictionary additionalInfo)
    {
        upgrades = new PlayerUpgrade(playerUpgrades);
        SavedInPath = additionalInfo;
    }

    public PlayerUpgrade upgrades;
    public Dictionary SavedInPath;
}
