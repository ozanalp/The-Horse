using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    public bool knocDownAttack;
    public float attackStrength;

    public GameObject otherObject;
    public GameObject enemyHealthUI;
    private Stats otherStats;
    private HorseController playerState;
    private EnemyState enemyState;

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "PlayerAttackBox" && other.tag == "EnemyHitBox")
        {
            // Collider bilgisini fonksiyona geçirebilmek için
            EnemyTakeDamage(other.gameObject);
            enemyHealthUI = GameObject.FindGameObjectWithTag("EnemyHealthUI");
            enemyHealthUI.gameObject.SetActive(true);
        }
        else if (gameObject.tag == "EnemyAttackBox" && other.tag == "PlayerHitBox")
        {
            // Aşağıdaki gibi sadece (other) yazarsak Player HitBox yazar
            // Debug.Log(other);
            PlayerTakeDamage(other.gameObject);
        }
        else
        {
            return;
        }
    }

    private void EnemyTakeDamage(GameObject other)
    {
        otherObject = other.transform.parent.gameObject;
        enemyState = otherObject.GetComponent<EnemyState>();
        otherStats = otherObject.GetComponent<Stats>();

        otherStats.health -= attackStrength;

        if (knocDownAttack == true)
        {
            enemyState.knockedDown = true;
        }
        else
        {
            enemyState.takingDamage = true;
        }

        Debug.Log(otherObject + " takes damage");
    }

    private void PlayerTakeDamage(GameObject other)
    {
        // OnTriggerEnter'dan gelen collider bilgisi bize burada 
        // Player HitBox root objesini, yani The Horse verir
        otherObject = other.transform.parent.gameObject;
        playerState = otherObject.GetComponent<HorseController>();
        otherStats = otherObject.GetComponent<Stats>();

        otherStats.health -= attackStrength;

        if (knocDownAttack == true)
        {
            playerState.knockedDown = true;
        }
        else
        {
            playerState.takingDamage = true;
        }

        Debug.Log(otherObject + " takes damage");
    }
}
