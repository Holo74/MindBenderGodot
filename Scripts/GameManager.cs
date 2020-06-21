using Godot;
using System;
using Godot.Collections;

//This is such a fucking mess that I need to fix and redo when the other parts of the game start to come into play
public class GameManager : Node
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    public Node Root { get { return GetTree().Root; } }
    public bool playing = false;
    [Signal]
    public delegate void ToggleGame(bool state);
    [Signal]
    public delegate void ReturnToTitle();
    //This is going to be the options menu
    private string savePath = "user://saveData.save";
    public string startingAreaPath;
    public SavedData[] datas = new SavedData[3];
    public bool SavesLoaded = false;

    public override void _Ready()
    {
        instance = this;
    }

    public void StartGame(string loadArea = "res://Scenes/Base.tscn")
    {
        startingAreaPath = loadArea;
        GetTree().ChangeScene("res://Scenes/WorldManager.tscn");
        File saveData = new File();
        saveData.Open(savePath, File.ModeFlags.Write);
        saveData.StoreLine("Meow");
        saveData.Close();
    }

    //Needs to be checked if in an actual game or can be done on the player as a call
    public void ToggleGamePause()
    {
        if (!PlayerController.CharacterPlaying())
            return;
        playing = !playing;
        if (playing)
        {
            GetTree().Paused = false;
            Input.SetMouseMode(Input.MouseMode.Captured);
        }
        else
        {
            GetTree().Paused = true;
            Input.SetMouseMode(Input.MouseMode.Visible);
        }
        EmitSignal(nameof(ToggleGame), playing);
    }

    public void SetToPlay()
    {
        if (!PlayerController.CharacterPlaying())
            return;
        playing = true;
        GetTree().Paused = false;
        Input.SetMouseMode(Input.MouseMode.Captured);
        EmitSignal(nameof(ToggleGame), playing);
    }

    public void QuitToMenu()
    {
        GetTree().Paused = false;
        Input.SetMouseMode(Input.MouseMode.Visible);
        GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
        EmitSignal(nameof(ReturnToTitle));
    }

    public void QuitGame()
    {
        GetTree().Quit();
    }
}
