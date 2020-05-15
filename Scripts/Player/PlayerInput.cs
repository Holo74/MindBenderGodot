using Godot;
using System;

public class PlayerInput : BaseAttatch
{
    private float timeDelta;
    InputHandler input { get { return InputHandler.Instance; } }
    PlayerOptions options { get { return controller.options; } }
    public PlayerInput(PlayerController controller) : base(controller, true) { }
    private bool sprintToggleOn = false;
    private bool sprintLock = false;

    public override void Update(float delta)
    {
        timeDelta = delta;
        bool sprint = SprintOutput();
        bool movedForwardOrBack = false;
        bool jumped = false;
        if (input.GetInput(Keys.moveForward))
        {
            controller.ability.Move(-controller.Transform.basis.z, sprint, true);
            movedForwardOrBack = true;
        }
        if (input.GetInput(Keys.moveBack))
        {
            controller.ability.Move(controller.Transform.basis.z, sprint);
            movedForwardOrBack = true;
        }
        if (input.GetInput(Keys.moveLeft))
        {
            controller.ability.Move(-controller.Transform.basis.x, sprint);
            if (input.GetInput(Keys.strafe) && !movedForwardOrBack)
                controller.ability.Strafe(-controller.Transform.basis.x);
        }
        if (input.GetInput(Keys.moveRight))
        {
            controller.ability.Move(controller.Transform.basis.x, sprint);
            if (input.GetInput(Keys.strafe) && !movedForwardOrBack)
                controller.ability.Strafe(controller.Transform.basis.x);
        }
        if (input.GetInput(Keys.jump))
        {
            controller.ability.Jump();
            jumped = true;
        }
        if (input.GetInput(Keys.gliding) && !jumped)
        {
            controller.ability.Glide();
        }
        if (input.GetInput(Keys.crouch))
            controller.ability.Crouch();
        if (input.GetInput(Keys.throwing))
        {
            controller.ability.Throw();
        }
    }

    private bool SprintOutput()
    {
        if (options.toggleSprint)
        {
            if (!sprintLock)
            {
                if (input.GetInput(Keys.sprint))
                {
                    sprintLock = true;
                    sprintToggleOn = !sprintToggleOn;
                }
            }
            else
            {
                if (!input.GetInput(Keys.sprint))
                    sprintLock = false;
            }
            return sprintToggleOn;
        }
        return input.GetInput(Keys.sprint);
    }

    public void Rotating(Vector2 vec)
    {
        if (GameManager.Instance.playing)
        {
            controller.bodyRotation.RotateAmount(vec.x * timeDelta * options.mouseXSensitivity);
            controller.headRotation.RotateAmount(vec.y * timeDelta * options.mouseYSensitivity);
        }

    }
}
