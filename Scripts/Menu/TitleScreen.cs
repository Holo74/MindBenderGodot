using Godot;
using System;

//The title screen that currently needs the options menu and loading menu and other fancy stuff
public class TitleScreen : Node
{

    public Node mainNode;

    public override void _Ready()
    {
        mainNode = GetChild(0);
        mainNode.GetChild(0).Connect("pressed", this, nameof(PlayNewGame));
        mainNode.GetChild(1).Connect("pressed", this, nameof(GoToDebug));
    }

    public void PlayNewGame()
    {
        GameManager.Instance.StartGame("res://Scenes/Start.tscn");
    }

    public void GoToDebug()
    {
        GameManager.Instance.StartGame();
    }

    public void MakeMainMenu(Node parent)
    {

    }
}
