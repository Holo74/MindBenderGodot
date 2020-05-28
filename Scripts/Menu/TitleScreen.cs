using Godot;
using System;

//The title screen that currently needs the options menu and loading menu and other fancy stuff
public class TitleScreen : Node
{

    public override void _Ready()
    {
        GetChild(0).Connect("pressed", this, nameof(PlayNewGame));
        GetChild(1).Connect("pressed", this, nameof(GoToDebug));
    }

    public void PlayNewGame()
    {
        GameManager.Instance.StartGame("res://Scenes/Start.tscn");
    }

    public void GoToDebug()
    {
        GameManager.Instance.StartGame();
    }
}
