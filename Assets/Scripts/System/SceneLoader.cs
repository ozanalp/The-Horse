using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    private Scene activeScene;
    public bool activeDeactive;

    private void Start()
    {
        activeScene = SceneManager.GetActiveScene();
        activeDeactive = gameObject.GetComponentInChildren<Toggle>().isOn;
        activeDeactive = false;
    }
    public void SwitchOnOffUICanvas()
    {
        if (activeDeactive)
        {
            Canvas uiCanvas = GetComponentInChildren<Canvas>();
            uiCanvas.enabled = true;
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            Canvas uiCanvas = GetComponentInChildren<Canvas>();
            uiCanvas.enabled = false;
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        Canvas uiCanvas = GetComponentInChildren<Canvas>();
        uiCanvas.enabled = false;
        activeDeactive = false;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(activeScene.buildIndex);
        Time.timeScale = 1;
        Canvas uiCanvas = GetComponentInChildren<Canvas>();
        uiCanvas.enabled = false;
        activeDeactive = false;
    }
}
