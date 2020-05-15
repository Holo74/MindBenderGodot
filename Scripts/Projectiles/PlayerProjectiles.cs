using Godot;
using System;

public class PlayerProjectiles : Area
{
    private float time = 0;
    [Export]
    private float speed = 1.2f;
    [Export]
    private float damage = 1;
    private Vector3 direction;
    public override void _EnterTree()
    {
        time = 0;
        direction -= Transform.basis.z;
    }

    public override void _Ready()
    {
        Connect("body_entered", this, nameof(HitTarget));
    }

    public void HitTarget(Node body)
    {
        if (body.Name == "Player")
        {
            ((HealthBase)body).TakeDamage(damage, DamageType.debug);
            GD.Print("Hey Player");
        }
    }

    public override void _Process(float delta)
    {
        time += delta;
        Translation += direction * delta * speed;
        if (time > 50000)
        {
            if (PlayerController.Instance != null)
            {
                PlayerController.Instance.ability.temp.Push(this);
            }
            else
            {
                QueueFree();
            }
        }
    }
}
