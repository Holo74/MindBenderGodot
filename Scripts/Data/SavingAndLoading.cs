using Godot;
using Godot.Collections;
using System;

public class SavingAndLoading
{
    // public void LoadingData()
    // {
    //     File saveData = new File();
    //     saveData.Open(savePath, File.ModeFlags.Write);
    //     saveData.StoreLine(JSON.Print(PlayerOptions.Instance.GetData()));
    //     saveData.StoreLine(JSON.Print(PlayerController.Instance.upgrades.GetAllUpgrades()));
    //     saveData.Close();
    // }

    // public void LoadingData()
    // {
    //     File saveData = new File();
    //     if (saveData.FileExists(savePath))
    //     {
    //         saveData.Open(savePath, File.ModeFlags.Read);
    //         Dictionary newOptions = (Dictionary)JSON.Parse(saveData.GetLine()).Result;
    //         playerUpgrades = (Dictionary)JSON.Parse(saveData.GetLine()).Result;
    //         PlayerOptions.Instance.SetData(newOptions);
    //     }
    //     saveData.Close();
    // }
}
