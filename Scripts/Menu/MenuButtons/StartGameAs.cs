using Godot;
using System;

public class StartGameAs : Button
{
    [Export]
    private int saveSlot;
    public override void _Ready()
    {
        Text = GameManager.Instance.datas[saveSlot].SavedInPath[SavedData.SavedGameName].ToString();
        Connect("pressed", this, nameof(LoadSave));
    }

    private void LoadSave()
    {
        GameManager.Instance.StartGame(saveSlot);
    }
}
