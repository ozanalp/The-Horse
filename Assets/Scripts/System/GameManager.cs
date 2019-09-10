using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Canvas UICanvas;
    public GameObject[] enemyArray;
    private Stats stats;
    SceneLoader sceneLoader;

    //public GameObject player;

    public float resetTime = 2f;
    public float timeSlowDown = .5f;

    public static GameManager instance = null;

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        UICanvas.enabled = false;
        stats = GetComponent<Stats>();
        sceneLoader = GetComponent<SceneLoader>();
    }

    private void Update()
    {
        enemyArray = GameObject.FindGameObjectsWithTag("Grabbable");

        //for (int i = 0; i < enemyArray.Length; i++)
        //{
        //    //grabbableDistance = (float)Math.Floor(Vector3.Distance(transform.position, FindObjectOfType<Grabbable>().transform.position) * 100) / 100;
        //    float distance = Vector3.Distance(enemyArray[i].transform.position, player.transform.position);
        //    Debug.Log("The element " + i + " is " + distance + " units away.");
        //}

        if (enemyArray.Length == 0)
        {
            CameraController.isFollowing = true;
        }
        UICanvasHandling();
    }

    public void GameOver()
    {
        Invoke("Reset", resetTime);
    }
    private void Reset()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UICanvasHandling()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            sceneLoader.activeDeactive = !sceneLoader.activeDeactive;
            GetComponent<SceneLoader>().SwitchOnOffUICanvas();
        }
    }

    private void DeathHandling()
    {
        if (stats.health <= 0)
        {
            GetComponent<SceneLoader>().SwitchOnOffUICanvas();
        }
    }
}
