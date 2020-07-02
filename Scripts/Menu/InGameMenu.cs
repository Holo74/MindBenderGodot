using Godot;
using System;

//An in game menu that controls the settings and the regular hud
public class InGameMenu : MenuBase
{
    [Export]
    private NodePath pathToQuit, pathToHealthConatiner;
    private Control hud, menu, healthContainer, savingRequest, savingCompleting;
    private ProgressBar healthBar;
    private RichTextLabel displayText;
    private static InGameMenu instance;
    public static InGameMenu Instance { get { return instance; } }
    private float timer = 4f, savingBuffer = 1f;
    private bool saving = false;
    public string inputMapAction = "";
    public override void _Ready()
    {
        GameManager.Instance.currentMenu = this;
        foreach (Control c in GetChildren())
        {
            c.Visible = false;
        }
        GameManager.Instance.Connect(nameof(GameManager.ToggleGame), this, nameof(ToggleMenu));
        PlayerController.Instance.Connect(nameof(PlayerController.TakingDamage), this, nameof(UpdateHealth));
        hud = GetChild<Control>(0);
        menu = GetChild<Control>(1);
        savingRequest = GetChild<Control>(3);
        savingCompleting = GetChild<Control>(4);
        mainNode = menu;
        healthContainer = GetNode<Control>(pathToHealthConatiner);
        healthBar = healthContainer.GetChild<ProgressBar>(0);
        displayText = GetChild(0).GetChild<RichTextLabel>(5);
        instance = this;

    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("Quit") && PlayerController.CharacterPlaying() && GameManager.Instance.allowInputs)
        {
            GameManager.Instance.ToggleGamePause();
        }
        if (saving)
        {
            savingBuffer -= delta;
            if (savingBuffer < 0)
            {
                saving = false;
                GameManager.Instance.SaveGame(WorldManager.instance.GetCurrentRoomFile());
            }
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (timer < 3f)
        {
            timer += delta;
            if (timer > 3f)
            {
                displayText.BbcodeText = "";
            }
        }
    }

    public void ToggleMenu(bool state)
    {
        if (state)
        {
            hud.Visible = true;
            mainNode.Visible = false;
            SettingsOptions.ResetNewSettings();
            mainNode = menu;
        }
        else
        {
            hud.Visible = false;
            mainNode.Visible = true;
        }
    }

    public static void DisplayText(string text)
    {
        instance.displayText.BbcodeText = "[center]" + text + "[/center]";
        instance.timer = 0f;
    }

    public void UpdateHealth(float newValue)
    {
        healthBar.Value = (newValue - 1) % 100;
    }

    public void ReadyMenu()
    {
        hud.Visible = true;
    }

    public void SaveGameConfirmed(bool saved)
    {
        if (saved)
        {
            mainNode.Visible = false;
            mainNode = savingCompleting;
            mainNode.Visible = true;
            saving = true;
            savingBuffer = 1f;
        }
        else
        {
            GameManager.Instance.ToggleGamePause();
        }
    }

    public void SaveRequest()
    {
        mainNode = savingRequest;
        GameManager.Instance.ToggleGamePause();
    }
}
