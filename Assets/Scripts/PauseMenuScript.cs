using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    
    private SceneChangeScript _sceneChangeScript;

    private void Awake()
    {
        _sceneChangeScript = FindObjectOfType<SceneChangeScript>();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void QuitToMain()
    {
        Time.timeScale = 1f;
        _sceneChangeScript.ReturnToMain();
    }

    public void Settings()
    {
        Time.timeScale = 1f;
        _sceneChangeScript.OpenSettings();
    }
}
