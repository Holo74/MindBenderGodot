using Godot;
using System;

//This is the base player class that is used to connect all the classes of the player and to house all of the nodes that are children of this node
public class PlayerController : HealthKinematic
{
    private float timeDelta;
    public delegate void Updating(float delta);
    private event Updating update, physicsUpdate;
    public PlayerOptions options { get; private set; }
    public Rotation headRotation { get; private set; }
    public Rotation bodyRotation { get; private set; }
    public Momentum playMovement { get; private set; }
    public SizeHandler size { get; private set; }
    public PlayerAbility ability { get; private set; }
    public PlayerInput inputs { get; private set; }
    public CameraRotHandler camRot { get; private set; }
    public PlayerUpgrade upgrades = new PlayerUpgrade();
    public Camera camera;
    [Export]
    private NodePath headPath, headRotationPath, cameraPath;
    [Signal]
    public delegate void TakingDamage();
    public static PlayerController Instance { get; private set; }

    private bool characterReady = false;

    public void ReadyPlayer(Vector3 spawn, Vector3 rotation)
    {
        Translate(spawn);
        Rotation = rotation;
        characterReady = true;
    }

    public override void _Ready()
    {
        Instance = this;
        options = PlayerOptions.Instance;
        headRotation = new Rotation(this, true, GetChild<Spatial>(2).GetChild<Spatial>(0), true, -80, 85);
        camera = GetChild<Spatial>(2).GetChild<Spatial>(0).GetChild<Camera>(0);
        bodyRotation = new Rotation(this, false, this);
        InputHandler.Instance.ConnectToMouseMovement(this, nameof(Rotating));
        playMovement = new Momentum(this);
        size = new SizeHandler(this, GetChild<Spatial>(2));

        PlayerAreaSensor.GetPlayerSensor(AreaSensorDirection.Bottom).RegisterStateChange(this, nameof(GroundChanging));
        ability = new PlayerAbility(this);
        inputs = new PlayerInput(this);
        camRot = new CameraRotHandler(this);
        Init(100);
    }

    public void UpdateCharacterSettings()
    {
        camera.Fov = SettingsOptions.cameraFOV;
    }

    public override void _Process(float delta)
    {
        if (GameManager.Instance.playing && characterReady)
            update?.Invoke(delta);
    }

    public override void _PhysicsProcess(float delta)
    {
        if (GameManager.Instance.playing && characterReady)
            physicsUpdate?.Invoke(delta);
    }

    public static bool CharacterPlaying()
    {
        if (Instance == null)
            return false;
        return Instance.characterReady;
    }

    public void AddToUpdate(Updating adding)
    {
        update += adding;
    }

    public void AddToPhysicsUpdate(Updating adding)
    {
        physicsUpdate += adding;
    }

    public void Rotating(Vector2 vec)
    {
        if (inputs != null)
            inputs.Rotating(vec);
    }

    public void GroundChanging(bool state)
    {
        ability.Land(state);
        playMovement.LandingSignal(state);
    }

    public override bool TakeDamage(float damage, DamageType typing)
    {
        Damaged(damage);
        EmitSignal(nameof(TakingDamage), GetHealth());
        GD.Print(GetHealth());
        return true;
    }

    public void DeloadPlayer()
    {
        Instance = null;
    }
}
