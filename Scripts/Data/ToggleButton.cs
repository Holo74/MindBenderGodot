using Godot;
using System;

public class ToggleButton : Node
{
    [Export]
    private int optionsValue;
    private CheckButton button;
    public override void _Ready()
    {
        button = GetChild<CheckButton>(1);
        button.Pressed = GameManager.GetBoolValueOf(optionsValue);
        Godot.Collections.Array passing = new Godot.Collections.Array();
        passing.Add(optionsValue);
        button.Connect("toggled", GameManager.Instance, nameof(GameManager.ChangeBoolValueOf), passing);
    }
}
