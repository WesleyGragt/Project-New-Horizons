using System.Drawing;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider masterVol, musicVol, sfxVol, sensitivity;
    public AudioMixer MainAudioMixer;
    public SettingsObject settings;
    public TMP_Dropdown controlDropDown;

    [Space]
    public Rotate rotateScript;
    public Gyro gyroScript;

    private void Start()
    {
        controlDropDown.value = settings.controlInt;

        masterVol.value = settings.masterVol;
        musicVol.value = settings.musicVol;
        sensitivity.value = settings.sensitivity;
        sfxVol.value = settings.sfxVol;

        gameObject.SetActive(false);
    }

    public void ChangeMasterVolume()
    {
        settings.masterVol = masterVol.value;
        MainAudioMixer.SetFloat("MasterVol", -Mathf.Log10(math.abs(masterVol.value)) * 20);
    }

    public void ChangeMusicVolume()
    {
        settings.musicVol = musicVol.value;
        MainAudioMixer.SetFloat("MusicVol", -Mathf.Log10(math.abs(musicVol.value)) * 20);
    }

    public void ChangeSFXVolume()
    {
        settings.sfxVol = sfxVol.value;
        MainAudioMixer.SetFloat("SFXVol", -Mathf.Log10(math.abs(sfxVol.value)) * 20);
    }

    public void SetSensitivity()
    {
        settings.sensitivity = sensitivity.value;
    }

    public void SetControls()
    {
        if (rotateScript == null || gyroScript == null) return;

        if (settings.controlInt == (int)ControlSetting.Joystick)
        {
            rotateScript.enabled = true;
            gyroScript.enabled = false;
        }
        else if (settings.controlInt == (int)ControlSetting.Gyro)
        {
            rotateScript.enabled = false;
            gyroScript.enabled = true;
        }
    }
}
