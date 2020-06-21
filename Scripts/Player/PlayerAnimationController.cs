using Godot;
using System;

public class PlayerAnimationController : BaseAttatch
{
    public PlayerAnimationController(PlayerController controller) : base(controller, false)
    {
        controller.AttachToDeath(PlayDeathAnimation);
        controller.animationNode.Active = true;
    }

    private void PlayDeathAnimation()
    {
        controller.animationNode.Set("parameters/conditions/Died", true);
    }
}
