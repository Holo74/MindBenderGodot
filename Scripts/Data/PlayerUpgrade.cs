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
    public static string FirstWeapon = "PistolBase";
    public static string SecondWeapon = "Rifle";
    public static string ThirdWeapon = "MiningDrill";
    public static string FourthWeapon = "EtherialGun";

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
        //Used for debugging and testing
        //Testing();
        //Decomment for actual builds and final release
        ActualUse();
    }

    public PlayerUpgrade(Dictionary currentUpgrades)
    {
        upgrades = currentUpgrades;
    }

    private void Testing()
    {
        upgrades.Add(DoubleJump, true);
        upgrades.Add(WallRunning, true);
        upgrades.Add(Sprinting, true);
        upgrades.Add(TripleJump, true);
        upgrades.Add(WallRunTime01, true);
        upgrades.Add(StrafeJump, true);
        upgrades.Add(Slide, true);
        upgrades.Add(Glide, true);
        upgrades.Add(FirstWeapon, true);
        upgrades.Add(SecondWeapon, true);
        upgrades.Add(ThirdWeapon, true);
        upgrades.Add(FourthWeapon, true);
    }

    private void ActualUse()
    {
        upgrades.Add(DoubleJump, false);
        upgrades.Add(WallRunning, false);
        upgrades.Add(Sprinting, false);
        upgrades.Add(TripleJump, false);
        upgrades.Add(WallRunTime01, false);
        upgrades.Add(StrafeJump, false);
        upgrades.Add(Slide, false);
        upgrades.Add(FirstWeapon, false);
        upgrades.Add(SecondWeapon, false);
        upgrades.Add(ThirdWeapon, false);
        upgrades.Add(FourthWeapon, false);
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
