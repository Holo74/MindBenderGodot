using Godot;
using System;

public class InGameMenu : Control
{
    [Export]
    private NodePath pathToQuit, pathToRespawn, pathToHealthConatiner;
    private Control hud, menu, healthContainer;
    private ProgressBar healthBar;
    public override void _Ready()
    {
        GameManager.Instance.Connect(nameof(GameManager.ToggleGame), this, nameof(ToggleMenu));
        GetNode(pathToQuit).Connect("pressed", GameManager.Instance, nameof(GameManager.QuitGame));
        GetNode(pathToRespawn).Connect("pressed", GameManager.Instance, nameof(GameManager.RespawnPlayer));
        PlayerController.Instance.Connect(nameof(PlayerController.TakingDamage), this, nameof(UpdateHealth));
        hud = GetChild<Control>(0);
        menu = GetChild<Control>(1);
        healthContainer = GetNode<Control>(pathToHealthConatiner);
        healthBar = healthContainer.GetChild<ProgressBar>(0);
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

    public void UpdateHealth(float newValue)
    {
        healthBar.Value = (newValue - 1) % 100;
    }
}
