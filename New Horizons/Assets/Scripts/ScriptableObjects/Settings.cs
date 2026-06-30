using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SettingsObject", order = 1)]
public class SettingsObject : ScriptableObject
{
    ControlSetting control = ControlSetting.Joystick;
    public float sensitivity = 0.9f;

    private Rotate rotateScript;
    private Gyro gyroScript;

    public void ToggleControl(int value)
    {
        control = (ControlSetting)value;

        if (rotateScript == null || gyroScript == null)
        {
            rotateScript = FindFirstObjectByType<Rotate>();
            gyroScript = FindFirstObjectByType<Gyro>();

            if (rotateScript == null || gyroScript == null) return;
        }

        if (control == ControlSetting.Joystick)
        {
            rotateScript.enabled = true;
            gyroScript.enabled = false;
        }
        else if (control == ControlSetting.Gyro)
        {
            rotateScript.enabled = false;
            gyroScript.enabled = true;
        }
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
