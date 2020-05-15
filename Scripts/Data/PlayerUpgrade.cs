using Godot.Collections;
using Godot;

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
        upgrades[name] = data;
    }

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
        return (bool)upgrades[name];
    }

    public float GetWallRunTotalTime()
    {
        float minTime = PlayerOptions.wallRunStickTime;
        if ((bool)upgrades[WallRunTime01])
        {
            minTime += minTime;
        }
        return minTime;
    }
}
