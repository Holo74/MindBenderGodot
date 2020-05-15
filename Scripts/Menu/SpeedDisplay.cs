using Godot;
using System;

public class SpeedDisplay : Label
{
    public override void _Process(float delta)
    {
        Text = "Speed: " + (PlayerController.Instance.playMovement.GetCurrentSpeed() * PlayerController.Instance.playMovement.GetAccelerate());
    }
}
