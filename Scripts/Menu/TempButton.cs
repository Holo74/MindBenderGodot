using Godot;
using System;

public class TempButton : Node
{
    private bool updated = false;
    private CheckButton button;
    [Export]
    private string nameOfUpgrade = "";

    public override void _Ready()
    {
        button = GetChild<CheckButton>(1);
        Godot.Collections.Array passing = new Godot.Collections.Array();
        passing.Add(nameOfUpgrade);
        button.Connect("toggled", GameManager.Instance, nameof(GameManager.UpdateUpgrade), passing);
    }

    public override void _Process(float delta)
    {
        if (!updated)
        {
            if (PlayerController.Instance != null)
            {
                updated = true;
                button.Pressed = PlayerController.Instance.upgrades.GetUpgrade(nameOfUpgrade);
            }
        }
    }
}
