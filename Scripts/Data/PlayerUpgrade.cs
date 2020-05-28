using Godot.Collections;
using Godot;

///<summary>The central thing that says what upgrades the player currenly has</summary>
public class PlayerUpgrade
{
    private Dictionary upgrades = new Dictionary();
    public static string DoubleJump = "doubleJump";
    public static string TripleJump = "tripleJump";
    public static string WallRunning = "wallRunning";
    public static string Sprinting = "sprinting";
    public static string WallRunTime01 = "wallRunTime01";
    public static string StrafeJump = "strafeJump";
    public static string Slide = "slide";
    public static string Glide = "glide";

    public Dictionary GetAllUpgrades()
    {
        return upgrades;
    }

    public void SetUpgradeTo(string name, bool data)
    {
        if (upgrades == null)
        {
            upgrades.Add(name, data);
        }
        else
        {
            upgrades[name] = data;
        }

    }
    //Used to populate the dictionary without getting an error when the player attempts to get them
    public PlayerUpgrade()
    {
        upgrades.Add(DoubleJump, false);
        upgrades.Add(WallRunning, false);
        upgrades.Add(Sprinting, false);
        upgrades.Add(TripleJump, false);
        upgrades.Add(WallRunTime01, false);
        upgrades.Add(StrafeJump, false);
        upgrades.Add(Slide, false);
        upgrades.Add(Glide, false);
    }

    public void LoadUpgrades(Dictionary newData)
    {
        upgrades = new Dictionary(newData);
    }

    public bool GetUpgrade(string name)
    {
        if (upgrades == null)
            return false;
        return (bool)upgrades[name];
    }

    public float GetWallRunTotalTime()
    {
        if (upgrades == null)
            return 0;
        float minTime = PlayerOptions.wallRunStickTime;
        if ((bool)upgrades[WallRunTime01])
        {
            minTime += minTime;
        }
        return minTime;
    }
}
