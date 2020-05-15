using Godot;
using System;

public class StateShower : Label
{
    public override void _Process(float delta)
    {
        if (PlayerController.Instance != null)
            Text = PlayerController.Instance.ability.GetCurrentState().ToString();
    }
}
