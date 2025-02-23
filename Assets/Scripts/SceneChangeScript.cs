using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SceneChangeScript : MonoBehaviour
{
    public FadeScript fadeScript;
    public SaveLoadScript saveLoadScript;

    public void CloseGame()
    {
        StartCoroutine(Delay("quit", -1, ""));
    }

    public void Play(int characterIndex, string name)
    {
        StartCoroutine(Delay("play", characterIndex, name));
    }

    public void OpenSettings()
    {
        // Запоминаем текущую сцену, чтобы можно было вернуться
        PlayerPrefs.SetInt("SceneToReturn", SceneManager.GetActiveScene().buildIndex);
        StartCoroutine(Delay("settings", -1, ""));
    }

    public void ReturnFromSettings()
    {
        // Загружаем сохранённый индекс сцены, если он есть
        int sceneToReturn = PlayerPrefs.GetInt("SceneToReturn", 0);
        StartCoroutine(Delay("return", sceneToReturn, ""));
    }

    public IEnumerator Delay(string command, int characterIndex, string name)
    {
        yield return fadeScript.FadeIn(0.1f);

        if (string.Equals(command, "quit", StringComparison.OrdinalIgnoreCase))
        {
            PlayerPrefs.DeleteAll();
            if (UnityEditor.EditorApplication.isPlaying)
                UnityEditor.EditorApplication.isPlaying = false;
            else
                Application.Quit();
        }
        else if (string.Equals(command, "play", StringComparison.OrdinalIgnoreCase))
        {
            saveLoadScript.SaveGame(characterIndex, name);
            SceneManager.LoadScene("Level1", LoadSceneMode.Single);
        }
        else if (string.Equals(command, "settings", StringComparison.OrdinalIgnoreCase))
        {
            SceneManager.LoadScene("Settings", LoadSceneMode.Single);
        }
        else if (string.Equals(command, "return", StringComparison.OrdinalIgnoreCase))
        {
            SceneManager.LoadScene(characterIndex, LoadSceneMode.Single);
        }
    }
}
