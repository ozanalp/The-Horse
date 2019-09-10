using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private Scene activeScene;

    private void Start()
    {
        activeScene = SceneManager.GetActiveScene();
    }
    public void Patreon()
    {
        Application.OpenURL("www.patreon.com/DragesAnimations");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        SceneManager.LoadScene(activeScene.buildIndex + 1);
        Time.timeScale = 1;
    }

}
