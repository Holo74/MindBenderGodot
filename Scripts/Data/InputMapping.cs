using Godot;
using System;
using Godot.Collections;

public class InputMapping : Node
{
    [Export]
    private string inputName = "Jump";
    public override void _Ready()
    {
        GetChild<Button>(0).Text = ((InputEvent)InputMap.GetActionList(inputName)[0]).AsText();
    }
}
