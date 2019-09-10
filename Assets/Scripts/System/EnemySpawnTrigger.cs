using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    public GameObject enemyPRF;

    public bool lastEnemy = false;


    private void Awake()
    {
        enemyPRF.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemySpawnTrigger")
        {
            enemyPRF.SetActive(true);
            if (lastEnemy)
            {
                Stats enemyStats = enemyPRF.GetComponent<Stats>();
                enemyStats.enemyToWin = true;
            }
        }
    }
}
