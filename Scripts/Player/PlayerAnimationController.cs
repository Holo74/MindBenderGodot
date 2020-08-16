using Godot;
using System;

public class PlayerAnimationController : BaseAttatch
{
    public PlayerAnimationController(PlayerController controller) : base(controller, false)
    {
        controller.AttachToDeath(PlayDeathAnimation);
        controller.animationNode.Active = true;
        controller.ability.AddToStateChange(PlayerStateAnimations);
    }

    private void PlayDeathAnimation()
    {
        controller.animationNode.Set("parameters/conditions/Died", true);
    }

    private void PlayerStateAnimations(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.walking:
                break;
            case PlayerState.standing:
                break;
            case PlayerState.wallRunning:
                break;
            case PlayerState.slide:
                break;
            case PlayerState.glide:
                break;
            default:
                break;
        }
    }
}
