using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public float health = 100;
    private HealthUI healthUI;
    private CharacterAnimation animationScript;
    private GameObject[] enemy;
    private bool characterDied; // iki taraf için de kullanılacak

    public bool isPlayer;

    private void Awake()
    {
        animationScript = GetComponentInChildren<CharacterAnimation>();

        // eğer enemy için de kullanacaksak bu kontrole gerek yok
        if (isPlayer)
        {
            healthUI = GetComponent<HealthUI>();
        }
    }

    private void Update()
    {
        enemy = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);
    }

    public void ApplyDamage(float damage, bool knockDown)
    {
        if (characterDied)
        {
            return;
        }

        health -= damage;

        //display health UI
        if (isPlayer)
        {
            healthUI.DisplayHealth(health);
        }

        if (health <= 0)
        {
            animationScript.Death();
            characterDied = true;

            // universal script.in player kısmında
            if (isPlayer)
            {
                foreach (GameObject enemyObject in enemy)
                {
                    enemyObject.GetComponentInParent<EnemyController>().attackPlayer = false;
                }
            }

            return;
        }

        // universal script.in enemy kısmında
        if (!isPlayer)
        {
            if (knockDown)
            {
                if (Random.Range(0, 2) > 0)
                {
                    animationScript.KnockDown();
                }
            }
            else
            {
                if (Random.Range(0, 3) > 1)
                {
                    animationScript.GettingHit();
                }
            }
        }
    }
}
