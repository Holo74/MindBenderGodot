using Godot;
using System;

public abstract class BaseAttatch
{
    protected float time = 0;
    protected PlayerController controller;
    public BaseAttatch(PlayerController controller, bool needsUpdate)
    {
        this.controller = controller;
        if (needsUpdate)
            controller.AddToUpdate(Update);
    }

    public virtual void Update(float delta) { time = delta; }
}
