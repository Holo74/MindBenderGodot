using Godot;
using System;

public class GlobalData : Node
{
    public static GlobalData Instance { get; private set; }
    public float MouseX { get; private set; }
    public float MouseY { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion motion)
        {
            MouseX = motion.Relative.x;
            MouseY = motion.Relative.y;
        }
    }
}
