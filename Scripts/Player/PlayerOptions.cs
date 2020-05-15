using Godot;
using System;
using Godot.Collections;

public class PlayerOptions : Node
{
    public static PlayerOptions Instance { get; private set; }
    public float mouseXSensitivity = -.2f;
    public float mouseYSensitivity = -.2f;
    public bool toggleSprint = false;
    public float cameraFOV = 70f;
    public const float playerWalkingSpeed = 10f;
    public const float playerMaxWalkingSpeed = 3.5f;
    //public float playerWalkingDamping = 5f; Was used for a different walking system
    public const float playerCrouchSpeed = 6f;
    public const float airMovementPush = 7f;
    public const float walkingDeceleration = 10f;
    public const float gravity = 13f;
    public const float jumpStr = 5f;
    public const float playerMaxCrouchSpeed = 2.4f;
    public const float playerMaxSprintSpeed = 8f;
    public const float playerSprintAcceleration = 5f;
    public const float playerDeccelerationAboveMax = 20.1f;
    public const float cayoteMaxTime = .2f;
    public const float doubleJumpStr = 5f;
    public const float tripleJumpStr = 5f;
    public const float wallRunningAngleAllowance = .65f;
    public const float wallRunningGravity = .2f;
    public const float wallJumpVerStr = 4f;
    public const float wallJumpHorStr = 4f;
    public const float wallRunStickTime = 2f;
    public const float playerStrafeStrengthHor = 10f;
    public const float playerStrafeStrengthVer = .5f;
    public const float slideStr = 2f;
    public const float slideMaxTime = 2f;
    public const float slideDeceleration = 2f;
    public const float glidePull = 13f;
    public const float glideFallSpeed = .5f;
    public void SetData(Dictionary data)
    {
        mouseXSensitivity = (float)data["mouseXSensitivity"];
        mouseYSensitivity = (float)data["mouseYSensitivity"];
        toggleSprint = (bool)data["toggleSprint"];
        cameraFOV = (float)data["cameraFOV"];

    }
    public Dictionary GetData()
    {
        Dictionary data = new Dictionary();
        data.Add("mouseXSensitivity", mouseXSensitivity);
        data.Add("mouseYSensitivity", mouseYSensitivity);
        data.Add("toggleSprint", toggleSprint);
        data.Add("cameraFOV", cameraFOV);
        return data;
    }
    public override void _Ready()
    {
        Instance = this;
    }
}
