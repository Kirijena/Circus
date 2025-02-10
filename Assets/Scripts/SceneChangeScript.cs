using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SceneChangeScript : MonoBehaviour
{
    public FadeScript fadeScript;
    
    public void closeGame()
    {
        StartCoroutine(Delay("quit", -1, ""));
    }

    public void openSettings()
    {
        StartCoroutine(Delay("settings", -1, ""));
    }

    public void closeSettings()
    {
        StartCoroutine(Delay("mainMenu", -1, ""));
    }

    public IEnumerator Delay(string command, int characterIndex, string name)
    {
        if(string.Equals(command, "quit", StringComparison.OrdinalIgnoreCase))
        {
            yield return fadeScript.FadeIn(0.1f);
            PlayerPrefs.DeleteAll();
            if (UnityEditor.EditorApplication.isPlaying)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
            else 
            { 
                Application.Quit();
            }
        } else if (string.Equals(command, "play", StringComparison.OrdinalIgnoreCase))
        {
            yield return fadeScript.FadeIn(0.1f);
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        else if (string.Equals(command, "settings", StringComparison.OrdinalIgnoreCase))
        {
            yield return fadeScript.FadeIn(0.1f);
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
        else if (string.Equals(command, "mainMenu", StringComparison.OrdinalIgnoreCase))
        {
            yield return fadeScript.FadeIn(0.1f);
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }
}
