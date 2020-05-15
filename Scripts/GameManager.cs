using Godot;
using System;
using Godot.Collections;

public class GameManager : Node
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    public Node Root { get { return GetTree().Root; } }
    public bool playing = false;
    [Signal]
    public delegate void ToggleGame(bool state);
    //This is going to be the options menu
    private string savePath = "user://saveData.save";
    Dictionary playerUpgrades;
    public Spatial respawnLocation;

    public override void _Ready()
    {
        instance = this;
        //Remove all of this and only load the data when you load a profile
        File saveData = new File();
        if (saveData.FileExists(savePath))
        {
            saveData.Open(savePath, File.ModeFlags.Read);
            Dictionary newOptions = (Dictionary)JSON.Parse(saveData.GetLine()).Result;
            playerUpgrades = (Dictionary)JSON.Parse(saveData.GetLine()).Result;
            PlayerOptions.Instance.SetData(newOptions);
        }
        saveData.Close();
        respawnLocation = (Spatial)GetTree().GetNodesInGroup("Spawn")[0];
    }
    private bool updatePlayer = false; //Remove this lock when proper saving is done
    public override void _Process(float delta)
    {
        //Update player needs to do some other stuff when loading the game file
        if (!updatePlayer)
        {
            updatePlayer = true;
            if (playerUpgrades != null)
                PlayerController.Instance.upgrades.LoadUpgrades(playerUpgrades);
        }
        //Put this on the player
        if (Input.IsActionJustPressed("Quit"))
        {
            ToggleGamePause();
        }
    }

    //Needs to be checked if in an actual game or can be done on the player as a call
    private void ToggleGamePause()
    {
        playing = !playing;
        if (playing)
        {
            GetTree().Paused = false;
            Input.SetMouseMode(Input.MouseMode.Captured);
            PlayerController.Instance.UpdateCharacterSettings(PlayerOptions.Instance);
        }
        else
        {
            GetTree().Paused = true;
            Input.SetMouseMode(Input.MouseMode.Visible);
        }
        EmitSignal(nameof(ToggleGame), playing);
    }

    //The three functions under neath need to be removed when values are set or they can be modified to actual options menu
    public void UpdateUpgrade(bool toggle, string nameOfUpgrade)
    {
        PlayerController.Instance.upgrades.SetUpgradeTo(nameOfUpgrade, toggle);
    }

    public void ChangeValueOf(float value, int i)
    {
        switch (i)
        {
            case 0:
                PlayerOptions.Instance.mouseXSensitivity = -value;
                break;
            case 1:
                PlayerOptions.Instance.mouseYSensitivity = -value;
                break;
            case 2:
                PlayerOptions.Instance.cameraFOV = value;
                break;
        }
    }

    public static float GetPlayerValue(int i)
    {
        switch (i)
        {
            case 0:
                return -PlayerOptions.Instance.mouseXSensitivity;
            case 1:
                return -PlayerOptions.Instance.mouseYSensitivity;
            case 2:
                return PlayerOptions.Instance.cameraFOV;
        }
        return 0;
    }

    //Don't delete until settings has been set up
    public static void ChangeBoolValueOf(bool value, int i)
    {
        switch (i)
        {
            case 3:
                PlayerOptions.Instance.toggleSprint = value;
                break;
        }
    }
    //Don't delete until settings has been set up
    public static bool GetBoolValueOf(int i)
    {
        switch (i)
        {
            case 3:
                return PlayerOptions.Instance.toggleSprint;
        }
        return false;
    }
    //Get rid of this and make an actual respawn system
    public void RespawnPlayer()
    {
        PlayerController.Instance.Translation = respawnLocation.Translation;
        PlayerController.Instance.playMovement.Stop();
        ToggleGamePause();
    }

    //Delete The inside and put into a saving game script
    public void QuitGame()
    {
        //Most of this needs to be put into a save station and not be done when quiting
        File saveData = new File();
        saveData.Open(savePath, File.ModeFlags.Write);
        saveData.StoreLine(JSON.Print(PlayerOptions.Instance.GetData()));
        saveData.StoreLine(JSON.Print(PlayerController.Instance.upgrades.GetAllUpgrades()));
        saveData.Close();
        GetTree().Quit();
    }
}
