using Godot;
using System;

//Currently not actually working because the door just kind of vanishes when you shoot it
public class Door : HealthStatic
{
    [Export]
    private string path;
    private float time;
    private AnimationTree tree;
    public override void _Ready()
    {
        Init(-1);
        tree = GetChild(1).GetChild<AnimationTree>(0);
        tree.Active = true;
    }

    public override void _Process(float delta)
    {
        if (time > 0)
        {
            if (time < 2)
            {
                tree.Set("parameters/conditions/Hit", false);
            }
            time -= delta;
            if (time < 0)
            {
                tree.Set("parameters/conditions/Time", true);
            }
        }
    }

    public override bool TakeDamage(float damage, DamageType typing, Node source)
    {
        //Currently the I need to figure out how the world is going to be pieced together so the doors will just open in tiny scenes with the animator
        //WorldManager.instance.LoadArea(path, GlobalTransform.origin, Rotation, DoorOpening);
        tree.Set("parameters/conditions/Time", false);
        tree.Set("parameters/conditions/Hit", true);
        time = 3f;
        return true;
    }

    public void DoorOpening()
    {
        QueueFree();
    }
}
