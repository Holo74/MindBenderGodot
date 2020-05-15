using Godot;
using System;

public class TempHolder : Control
{
    [Export]
    private NodePath pathToQuit, pathToRespawn;
    public override void _Ready()
    {
        GameManager.Instance.Connect(nameof(GameManager.ToggleGame), this, nameof(ToggleMenu));
        GetNode(pathToQuit).Connect("pressed", GameManager.Instance, nameof(GameManager.QuitGame));
        GetNode(pathToRespawn).Connect("pressed", GameManager.Instance, nameof(GameManager.RespawnPlayer));
    }
    public void ToggleMenu(bool state)
    {
        if (state)
        {
            Visible = false;
        }
        else
        {
            Visible = true;
        }
    }
}
