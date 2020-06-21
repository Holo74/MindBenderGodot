using Godot;
using System.Collections.Generic;
using System;

public class WorldManager : Node
{
    public static WorldManager instance { get; private set; }
    public ObjectPool<PlayerProjectiles> shots;
    public ObjectPool<EnemyProjectiles> enemyProjectilePool;
    private ResourceInteractiveLoader loader;
    private ProgressBar loadingBar;
    private Queue<string> loadingPaths = new Queue<string>();
    private float waitFrame = 1;
    private float loaded = 0;

    private Vector3 loadLocation, loadRotation;
    [Signal]
    public delegate void AreaLoaded();
    private AreaLoaded loadingDone;
    private Vector3 lastSaveLoc;
    //Do not rely on this telling you what room the player is actually in.  This is just to know what rooms are loaded and not to say which rooms the player is in
    private Node previousRoom, currentRoom, playerInRoom;
    private string previousRoomFile, currentRoomFile;
    public override void _Ready()
    {
        instance = this;
        loadingBar = GetChild(0).GetChild<ProgressBar>(1);
        loadingPaths.Enqueue("res://Scenes/Player.tscn");
        loadingPaths.Enqueue("res://Scenes/InGameMenu.tscn");
        loadingPaths.Enqueue(GameManager.Instance.startingAreaPath);
        currentRoomFile = GameManager.Instance.startingAreaPath;
        waitFrame = 1;
        loader = ResourceLoader.LoadInteractive(loadingPaths.Dequeue());
        shots = new ObjectPool<PlayerProjectiles>("TempProjectile.tscn");
        enemyProjectilePool = new ObjectPool<EnemyProjectiles>("EnemyTempProj.tscn");
        loaded = 0;
        GameManager.Instance.Connect(nameof(GameManager.ReturnToTitle), this, nameof(StoppingWorld));
    }

    public override void _Process(float delta)
    {
        if (waitFrame > 0)
        {
            waitFrame -= 1;
            if (loadingBar.Value == 100 && waitFrame <= 0)
            {
                Spatial spawn = new Spatial();
                if (GetTree().HasGroup("RespawnLocation"))
                    spawn = (Spatial)GetTree().GetNodesInGroup("RespawnLocation")[0];
                PlayerController.Instance.ReadyPlayer(spawn.GlobalTransform.origin, spawn.Rotation);
                PlayerController.Instance.UpdateCharacterSettings();
                RemoveChild(GetChild(0));
                InGameMenu.Instance.ReadyMenu();
                GameManager.Instance.SetToPlay();
            }
            return;
        }
        if (loader != null && !PlayerController.CharacterPlaying())
        {
            Error error = loader.Poll();
            switch (error)
            {
                case Error.FileEof:
                    PackedScene holder = loader.GetResource().Duplicate() as PackedScene;
                    loader.Dispose();
                    Node node = holder.Instance();
                    loader = null;
                    AddChild(node);
                    if (loadingPaths.Count == 0)
                    {
                        currentRoom = node;
                        playerInRoom = node;
                        loadingBar.Value = 100;
                        waitFrame = 10f;
                    }
                    else
                    {
                        loader = ResourceLoader.LoadInteractive(loadingPaths.Dequeue());
                    }
                    break;
                case Error.Ok:
                    float temp = (loader.GetStage() / (float)loader.GetStageCount()) * 100 / (float)(loadingPaths.Count + 1);
                    if (loaded < temp)
                        loaded = temp;
                    loadingBar.Value = loaded;
                    break;
                default:
                    loader = null;
                    GD.Print("Error");
                    break;
            }
        }
        if (loader != null && PlayerController.CharacterPlaying())
        {
            Error error = loader.Poll();
            switch (error)
            {
                case Error.FileEof:
                    PackedScene holder = loader.GetResource() as PackedScene;
                    Spatial node = (Spatial)holder.Instance();
                    loader = null;
                    node.Translate(loadLocation);
                    node.Rotation = loadRotation;
                    AddChild(node);
                    EmitSignal(nameof(AreaLoaded));
                    loadingDone?.Invoke();
                    loadingDone = null;
                    if (previousRoom != null)
                    {
                        if (previousRoom == playerInRoom)
                        {
                            previousRoom.QueueFree();
                            previousRoom = currentRoom;
                            previousRoomFile = currentRoomFile;
                        }
                        else
                        {
                            currentRoom.QueueFree();
                        }
                    }
                    currentRoom = node;
                    break;
                case Error.Ok:
                    break;
                default:
                    loader = null;
                    break;
            }
        }
    }

    public void LoadArea(string path, Vector3 loc, Vector3 rot)
    {
        if (currentRoomFile == path || previousRoomFile == path)
            return;
        loader = ResourceLoader.LoadInteractive(path);
        loadLocation = loc;
        loadRotation = rot;
        currentRoomFile = path;
    }

    public void LoadArea(string path, Vector3 loc, Vector3 rot, AreaLoaded load)
    {
        if (currentRoomFile == path || previousRoomFile == path)
            return;
        loadingDone += load;
        LoadArea(path, loc, rot);
    }

    public void StoppingWorld()
    {
        instance = null;
        PlayerController.Instance.DeloadPlayer();
        RayCastData.ClearDic();
        PlayerAreaSensor.ResetSensors();
    }
}
