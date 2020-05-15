using Godot;
using System;

public class PlayerController : HealthBase
{
    private float timeDelta;
    public delegate void Updating(float delta);
    private event Updating update;
    public PlayerOptions options { get; private set; }
    public Rotation headRotation { get; private set; }
    public Rotation bodyRotation { get; private set; }
    public Momentum playMovement { get; private set; }
    public SizeHandler size { get; private set; }
    public PlayerAbility ability { get; private set; }
    public PlayerInput inputs { get; private set; }
    public PlayerUpgrade upgrades = new PlayerUpgrade();
    public Camera camera;
    [Export]
    private NodePath headPath, headRotationPath, cameraPath;
    [Signal]
    public delegate void TakingDamage();
    public static PlayerController Instance { get; private set; }

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
        PlayerAreaSensor.areaSensors[AreaSensorDirection.Bottom].RegisterStateChange(this, nameof(GroundChanging));
        ability = new PlayerAbility(this);
        inputs = new PlayerInput(this);
        Init(100);
        EmitSignal(nameof(TakingDamage), GetHealth());
    }

    public void UpdateCharacterSettings(PlayerOptions settings)
    {
        camera.Fov = settings.cameraFOV;
    }

    public override void _Process(float delta)
    {
        if (GameManager.Instance.playing)
            update?.Invoke(delta);
    }

    public void AddToUpdate(Updating adding)
    {
        update += adding;
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
}
