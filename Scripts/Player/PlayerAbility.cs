using Godot;
using System;

//This controls how the abilities work and to process all the filtered inputs
public class PlayerAbility : BaseAttatch
{
    public PlayerAbility(PlayerController controller) : base(controller, true)
    {
    }

    public bool DoubleJumpUsed = false, tripleJumpUsed = false;
    public float cayoteTime = 0f;
    private PlayerState currentState = PlayerState.standing;
    private WallRunningData right = new WallRunningData(RayDirections.Right, AreaSensorDirection.Right);
    private WallRunningData left = new WallRunningData(RayDirections.Left, AreaSensorDirection.Left);
    public bool RunningOnRightWall = false;
    private WallRunningData attachedTo;
    private bool canSprint { get { return controller.upgrades.GetUpgrade(PlayerUpgrade.Sprinting); } }
    private float currentWallRunTime = 0f;
    public delegate void StateChange(PlayerState state);
    private StateChange stateChange;

    public void AddToStateChange(StateChange function)
    {
        stateChange += function;
    }

    public override void Update(float delta)
    {
        base.Update(delta);
        if (!PlayerAreaSensor.GetArea(AreaSensorDirection.Bottom) && cayoteTime < PlayerOptions.cayoteMaxTime)
        {
            cayoteTime += time;
        }
        if (currentState == PlayerState.wallRunning)
        {
            currentWallRunTime += time;
        }
        if (!glideLock)
        {
            if (currentState == PlayerState.glide)
            {
                ChangeState(PlayerState.empty);
            }
        }
        glideLock = false;
    }

    public void Move(Vector3 direction, bool sprint, bool wallRun = false)
    {
        switch (currentState)
        {
            case PlayerState.standing:
            case PlayerState.sprinting:
            case PlayerState.walking:
                float acceleration = PlayerOptions.playerWalkingSpeed;
                float maxSpeed = PlayerOptions.playerMaxWalkingSpeed;
                if (canSprint && sprint)
                {
                    acceleration = PlayerOptions.playerSprintAcceleration;
                    maxSpeed = PlayerOptions.playerMaxSprintSpeed;
                }
                if (controller.playMovement.GetCurrentSpeed() > controller.playMovement.GetMaxSpeed())
                {
                    acceleration = 0;
                }
                controller.playMovement.GroundMovement(direction, maxSpeed, acceleration);
                break;
            case PlayerState.crouch:
                controller.playMovement.GroundMovement(direction, PlayerOptions.playerMaxCrouchSpeed, PlayerOptions.playerWalkingSpeed);
                break;
            case PlayerState.wallRunning:
            case PlayerState.fallingDown:
                if (currentWallRunTime < controller.upgrades.GetWallRunTotalTime())
                {
                    if (controller.upgrades.GetUpgrade(PlayerUpgrade.WallRunning))
                    {
                        if (WallRun(wallRun))
                        {
                            break;
                        }
                    }
                }
                if (currentState == PlayerState.wallRunning)
                {
                    ChangeState(PlayerState.empty);
                }
                goto case PlayerState.fallingUp;
            case PlayerState.fallingUp:
                controller.playMovement.AirMovement(direction, PlayerOptions.airMovementPush);
                break;
            case PlayerState.glide:
                controller.playMovement.AirMovement(direction, PlayerOptions.glidePull);
                break;
            default:
                break;
        }
    }

    public void Strafe(Vector3 direction)
    {
        switch (currentState)
        {
            case PlayerState.standing:
            case PlayerState.walking:
            case PlayerState.sprinting:
            case PlayerState.empty:
                if (controller.upgrades.GetUpgrade(PlayerUpgrade.StrafeJump) && controller.playMovement.GetCurrentSpeed() < controller.playMovement.GetMaxSpeed() + 1f)
                {
                    controller.playMovement.VerticalIncrease(PlayerOptions.playerStrafeStrengthVer);
                    controller.playMovement.HorizontalAccelerationSet(direction * PlayerOptions.playerStrafeStrengthHor);
                }
                break;
        }
    }

    private bool WallRun(bool continuing)
    {
        if (!continuing)
            return false;
        RunningOnRightWall = right.Sensed();

        if (RunningOnRightWall || left.Sensed())
        {
            attachedTo = right.Sensed() ? right : left;
            if (attachedTo.Normals().Dot(controller.Transform.basis.z) > PlayerOptions.wallRunningAngleAllowance)
            {
                return false;
            }
            ChangeState(PlayerState.wallRunning);
            controller.playMovement.ChangeStableVectorDirection(attachedTo.Normals().Cross(Vector3.Up) * (attachedTo == right ? 1 : -1));
            controller.playMovement.SetPushing(-attachedTo.Normals() * 10);
            controller.playMovement.HorizontalAccelerationSet(Vector3.Zero);
            return true;
        }
        if (currentState == PlayerState.wallRunning)
        {
            ChangeState(PlayerState.empty);
            ResetJumps();
        }
        return false;
    }
    private bool glideLock = false;
    public void Glide()
    {
        switch (currentState)
        {
            case PlayerState.glide:
            case PlayerState.fallingDown:
                if (controller.upgrades.GetUpgrade(PlayerUpgrade.Glide))
                {
                    currentState = PlayerState.glide;
                    glideLock = true;
                }
                break;
        }
    }

    public void Jump()
    {
        switch (currentState)
        {
            case PlayerState.slide:
            case PlayerState.standing:
            case PlayerState.sprinting:
            case PlayerState.walking:
            case PlayerState.crouch:
                cayoteTime = PlayerOptions.cayoteMaxTime;
                controller.playMovement.VerticalIncrease(PlayerOptions.jumpStr);
                if (controller.size.crouched)
                    controller.size.Crouch();
                break;
            case PlayerState.wallRunning:
                controller.playMovement.VerticalIncrease(PlayerOptions.wallJumpVerStr);
                controller.playMovement.HorizontalAccelerationSet(PlayerOptions.wallJumpHorStr * attachedTo.Normals());
                ResetJumps();
                break;
            case PlayerState.fallingUp:
            case PlayerState.fallingDown:
                if (cayoteTime < PlayerOptions.cayoteMaxTime)
                {
                    cayoteTime = PlayerOptions.cayoteMaxTime;
                    controller.playMovement.VerticalIncrease(PlayerOptions.jumpStr);
                    return;
                }
                if (controller.upgrades.GetUpgrade(PlayerUpgrade.DoubleJump) && !DoubleJumpUsed)
                {
                    DoubleJumpUsed = true;
                    controller.playMovement.VerticalIncrease(PlayerOptions.doubleJumpStr);
                    return;
                }
                if (controller.upgrades.GetUpgrade(PlayerUpgrade.TripleJump) && !tripleJumpUsed)
                {
                    tripleJumpUsed = true;
                    controller.playMovement.VerticalIncrease(PlayerOptions.tripleJumpStr);
                    return;
                }
                break;
            default:
                break;
        }
    }

    private void ResetJumps()
    {
        DoubleJumpUsed = false;
        tripleJumpUsed = false;
        currentWallRunTime = 0f;
    }

    public void Land(bool state)
    {
        if (state)
        {
            ResetJumps();
            cayoteTime = 0f;
        }
    }

    public void Crouch()
    {
        switch (currentState)
        {
            case PlayerState.slide:
            case PlayerState.crouch:
                controller.playMovement.Accelerate();
                controller.size.Crouch();
                break;
            case PlayerState.standing:
            case PlayerState.walking:
                controller.size.Crouch();
                break;
            case PlayerState.sprinting:
                if (controller.upgrades.GetUpgrade(PlayerUpgrade.Slide))
                {
                    controller.playMovement.Accelerate(PlayerOptions.slideStr);
                }
                controller.size.Crouch();
                break;
        }
    }

    public void Throw()
    {
        //Need to reposition where the thing is thrown from and need to make it ignore the player.  
        //Along with having the cross hair change the size that it is depending on the distance you are from a target
        //Angle the object to the thing you are looking at
        WorldManager.instance.shots.Pull(controller.camera.GlobalTransform.origin, controller.camera.GlobalTransform.basis.GetEuler());
    }

    public PlayerState GetCurrentState()
    {
        return currentState;
    }
    public void ChangeState(PlayerState state)
    {
        if (currentState != state)
        {
            currentState = state;
            stateChange?.Invoke(state);
        }
    }
}

public class WallRunningData
{
    RayDirections ray;
    AreaSensorDirection sensorDirection;
    public WallRunningData(RayDirections direction, AreaSensorDirection sensor)
    {
        this.ray = direction;
        sensorDirection = sensor;
    }

    public Vector3 Normals()
    {
        return RayCastData.SurroundingCasts[ray].normal;
    }

    public bool Sensed()
    {
        return RayCastData.SurroundingCasts[ray].colliding && PlayerAreaSensor.GetArea(sensorDirection);
    }
}

public enum PlayerState
{
    walking,
    standing,
    wallRunning,
    fallingUp,
    fallingDown,
    empty,
    sprinting,
    crouch,
    glide,
    slide
}
