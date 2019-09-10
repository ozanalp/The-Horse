using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public float startingHealth;
    public float health;

    public bool displayUI;
    public bool enemyToWin;

    public Slider healthSlider;
    public GameObject healthUI;
    //public GameObject sliderSprite;
    public Image sliderBackground;
    private EnemyState enemyState;
    private GameManager gameManager;

    private void Awake()
    {
        health = startingHealth;
        enemyState = GetComponent<EnemyState>();
        gameManager = FindObjectOfType<GameManager>();
    }
    private void Update()
    {
        if (gameObject.tag == "Player")
        {
            healthUI = GameObject.FindGameObjectWithTag("PlayerHealthUI");
            healthSlider = healthUI.gameObject.transform.GetChild(0).GetComponent<Slider>();

            if (healthSlider.maxValue == 0)
            {
                healthSlider.maxValue = startingHealth;
            }
            healthSlider.value = health;
        }
        if (gameObject.tag == "Grabbable" && displayUI == false
            || GameObject.FindGameObjectWithTag("Grabbable") == null)
        {
            healthUI = GameObject.FindGameObjectWithTag("EnemyHealthUI");
            healthUI.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false;
            sliderBackground = healthUI.gameObject.GetComponentInChildren<Image>();
            sliderBackground.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false;
            //if (displayUI)
            //{
            //    healthUI.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
            //    sliderBackground.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
            //}
        }
        if (displayUI)
        {
            healthUI = GameObject.FindGameObjectWithTag("EnemyHealthUI");
            healthSlider = healthUI.gameObject.transform.GetChild(0).GetComponent<Slider>();
            healthUI.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
            sliderBackground = healthUI.gameObject.GetComponentInChildren<Image>();
            sliderBackground.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;

            if (healthSlider.maxValue == 0)
            {
                healthSlider.maxValue = startingHealth;
            }
            healthSlider.value = health;

            if (enemyState.takingDamage)
            {
                healthSlider = healthUI.gameObject.transform.GetChild(0).GetComponent<Slider>();
                sliderBackground = healthUI.gameObject.GetComponentInChildren<Image>();                
            }
            else
            {
                return;
            }

            if (healthSlider.maxValue == 0)
            {
                healthSlider.maxValue = startingHealth;
            }
            healthSlider.value = health;
        }
        else if (gameObject.tag == "Grabbable" && displayUI == false)
        {
            healthUI = null;
            healthSlider = null;
        }
        HealthBelowZero();
    }

    private void HealthBelowZero()
    {
        if (health <= 0)
        {
            Destroy(gameObject.transform.parent.gameObject);

            if (enemyToWin == true)
            {
                Time.timeScale = .5f;
                GameManager.instance.GameOver();
            }
        }
    }
}
