using Godot;
using System;

//An in game menu that controls the settings and the regular hud
public class InGameMenu : Control
{
    [Export]
    private NodePath pathToQuit, pathToHealthConatiner;
    private Control hud, menu, healthContainer;
    private ProgressBar healthBar;
    private RichTextLabel displayText;
    private static InGameMenu instance;
    public static InGameMenu Instance { get { return instance; } }
    private float timer = 4f;
    public override void _Ready()
    {
        GameManager.Instance.Connect(nameof(GameManager.ToggleGame), this, nameof(ToggleMenu));
        GetNode(pathToQuit).Connect("pressed", GameManager.Instance, nameof(GameManager.QuitToMenu));
        PlayerController.Instance.Connect(nameof(PlayerController.TakingDamage), this, nameof(UpdateHealth));
        hud = GetChild<Control>(0);
        menu = GetChild<Control>(1);
        healthContainer = GetNode<Control>(pathToHealthConatiner);
        healthBar = healthContainer.GetChild<ProgressBar>(0);
        displayText = GetChild(0).GetChild<RichTextLabel>(5);
        instance = this;
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("Quit") && PlayerController.CharacterPlaying())
        {
            GameManager.Instance.ToggleGamePause();
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
            menu.Visible = false;
        }
        else
        {
            hud.Visible = false;
            menu.Visible = true;
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
}
