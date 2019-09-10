using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float CurrentHealth { get; set; }
    public float MaxHealth { get; set; }

    public Slider healthbar;

    private void Start()
    {
        MaxHealth = 20f;
        CurrentHealth = MaxHealth;
        healthbar.value = CalculateHealth();
    }

    private void Update()
    {
        Grabbing grabbing = FindObjectOfType<Grabbing>();
        bool grabOn = grabbing.grabbed;
        if (grabOn)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                DealDamage(5);
            }
        }
    }

    private void DealDamage(float damageValue)
    {
        CurrentHealth -= damageValue;
        healthbar.value = CalculateHealth();
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private float CalculateHealth()
    {
        return CurrentHealth / MaxHealth;
    }

    private void Die()
    {
        CurrentHealth = 0;
        Debug.Log("You go with Jesus");
        //Restart();
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}