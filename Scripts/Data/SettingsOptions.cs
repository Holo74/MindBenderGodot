using Godot.Collections;
using System;

public class SettingsOptions
{
    public static float mouseXSensitivity = -.2f;
    public static float mouseYSensitivity = -.2f;
    public static bool toggleSprint = false;
    public static float cameraFOV = 70f;
    public static bool leanWhileRunning = true;
    public static void SetData(Dictionary data)
    {
        mouseXSensitivity = (float)data["mouseXSensitivity"];
        mouseYSensitivity = (float)data["mouseYSensitivity"];
        toggleSprint = (bool)data["toggleSprint"];
        cameraFOV = (float)data["cameraFOV"];

    }
    public static Dictionary GetData()
    {
        Dictionary data = new Dictionary();
        data.Add("mouseXSensitivity", mouseXSensitivity);
        data.Add("mouseYSensitivity", mouseYSensitivity);
        data.Add("toggleSprint", toggleSprint);
        data.Add("cameraFOV", cameraFOV);
        return data;
    }
}
