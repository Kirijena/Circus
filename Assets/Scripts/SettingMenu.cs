using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMPro.TMP_Dropdown dropdown;
    public Slider slider;
    public Toggle toggle;

    Resolution[] resolutions;

    private SceneChangeScript _sceneChangeScript;

    private void Awake()
    {
        _sceneChangeScript = FindObjectOfType<SceneChangeScript>();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    void Start()
    {
        float value;
        audioMixer.GetFloat("volume", out value);
        slider.value = value;

        toggle.isOn = Screen.fullScreen;

        resolutions = Screen.resolutions;
        
        dropdown.ClearOptions();
        
        List<string> options = new List<string>();
        int current = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                current = i;
            }
        }

        dropdown.AddOptions(options);
        dropdown.value = current;
        dropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ReturnButton()
    {
        string sceneToReturn = PlayerPrefs.GetString("SceneToReturn");
        Debug.Log(sceneToReturn);

        if (sceneToReturn == "game")
        {
            _sceneChangeScript.PlayAndLoad();
        }
        else
        {
            _sceneChangeScript.ReturnToMain();
        }
    }
}
