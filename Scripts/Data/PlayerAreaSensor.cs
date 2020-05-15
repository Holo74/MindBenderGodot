using Godot;
using System.Collections.Generic;

public class PlayerAreaSensor : Node
{
    [Export]
    private AreaSensorDirection direction;
    private int currentBodyCount;
    public static Dictionary<AreaSensorDirection, bool> area = new Dictionary<AreaSensorDirection, bool>();
    [Signal]
    public delegate void ChangedState(bool state);
    public static Dictionary<AreaSensorDirection, PlayerAreaSensor> areaSensors = new Dictionary<AreaSensorDirection, PlayerAreaSensor>();
    public override void _Ready()
    {
        area.Add(direction, false);
        areaSensors.Add(direction, this);
        Connect("body_entered", this, nameof(Entered));
        Connect("body_exited", this, nameof(Left));
    }

    public void Entered(Node body)
    {
        if (body.Name != "Player")
        {
            currentBodyCount++;
            if (!area[direction])
            {
                area[direction] = true;
                EmitSignal(nameof(ChangedState), true);
            }


        }
    }

    public void RegisterStateChange(Godot.Object target, string method)
    {
        Connect(nameof(ChangedState), target, method);
    }

    public void Left(Node body)
    {
        if (body.Name != "Player")
        {
            currentBodyCount--;
            if (currentBodyCount == 0)
            {
                area[direction] = false;
                EmitSignal(nameof(ChangedState), false);
            }
        }
    }
}

public enum AreaSensorDirection
{
    Left,
    Right,
    Bottom
}
