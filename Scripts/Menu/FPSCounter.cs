using Godot;
using System;
public class FPSCounter : Label
{
    public override void _Process(float delta)
    {
        Text = "FPS: " + Performance.GetMonitor(Performance.Monitor.TimeFps).ToString();
    }
}
