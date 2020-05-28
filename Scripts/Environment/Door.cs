using Godot;
using System;

//Currently not actually working because the door just kind of vanishes when you shoot it
public class Door : HealthStatic
{
    [Export]
    private string path;
    public override void _Ready()
    {
        Init(-1);
    }

    public override bool TakeDamage(float damage, DamageType typing)
    {
        WorldManager.instance.LoadArea(path, GlobalTransform.origin, Rotation, DoorOpening);
        return true;
    }

    public void DoorOpening()
    {
        QueueFree();
    }
}
