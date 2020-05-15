using Godot;
using System;

public class LeavingBoundsTestArea : Area
{

    public void Respawn(Node node)
    {
        if (node.Name == "Player")
            GameManager.Instance.RespawnPlayer();
    }
}
