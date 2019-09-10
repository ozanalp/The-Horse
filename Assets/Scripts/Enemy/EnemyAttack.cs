using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange;
    public float attackStartDelay;

    public GameObject spriteObject;

    public GameObject attackBox1;
    //public GameObject attackBox2;
    //public GameObject attackBox3;
    //public Transform shootPoint;
    public Sprite attackHitFrame1; //attackHitFrame2, attackHitFrame3;
    public Sprite currentSprite;
    private NavMeshAgent navMeshAgent;
    private EnemySight enemySight;
    private EnemyWalk enemyWalk;
    private EnemyState enemyState;
    private Animator animator;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemySight = GetComponent<EnemySight>();
        enemyWalk = GetComponent<EnemyWalk>();
        enemyState = GetComponent<EnemyState>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {

    }

    private void Update()
    {
        currentSprite = spriteObject.GetComponent<SpriteRenderer>().sprite;
        //if (enemySight.playerInSight && enemySight.playerDistance < attackRange)
        //{
        //    animator.SetBool("Attack", true);

        //    if (attackHitFrame1 == currentSprite)
        //    {
        //        attackBox1.gameObject.SetActive(true);
        //    }
        //    //else if (attackHitFrame2 == currentSprite)
        //    //{
        //    //    attackBox2.gameObject.SetActive(true);
        //    //}
        //    //else if (attackHitFrame3 == currentSprite)
        //    //{
        //    //    attackBox3.gameObject.SetActive(true);
        //    //}
        //    else
        //    {
        //        attackBox1.gameObject.SetActive(false);
        //        //attackBox2.gameObject.SetActive(false);
        //        //attackBox3.gameObject.SetActive(false);
        //    }
        //}
        if (enemyState.currentState == EnemyState.currentStateEnum.attack)
        {
            Attack();
        }
    }

    private void Attack()
    {
        navMeshAgent.ResetPath();

        if (attackHitFrame1 == currentSprite)
        {
            attackBox1.gameObject.SetActive(true);
        }
        //else if (attackHitFrame2 == currentSprite)
        //{
        //    attackBox2.gameObject.SetActive(true);
        //}
        //else if (attackHitFrame3 == currentSprite)
        //{
        //    attackBox3.gameObject.SetActive(true);
        //}
        else
        {
            attackBox1.gameObject.SetActive(false);
            //attackBox2.gameObject.SetActive(false);
            //attackBox3.gameObject.SetActive(false);
        }
    }
}
