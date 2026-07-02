using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SettingsObject", order = 1)]
public class SettingsObject : ScriptableObject
{
    ControlSetting control = ControlSetting.Joystick;
    public float sensitivity = 0.9f;

    public float masterVol;
    public float musicVol;
    public float sfxVol;

    private Rotate rotateScript;
    private Gyro gyroScript;

    public int controlInt = 0;

    public void ToggleControl(int value)
    {
        controlInt = value;
        control = (ControlSetting)value;
    }

    public void SetSensitivity(float value)
    {
        sensitivity = value;

        if (gyroScript != null) gyroScript.sensitivity = sensitivity;
    }
}

enum ControlSetting
{
    Joystick,
    Gyro
}
