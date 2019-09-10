using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyState : MonoBehaviour
{
    private NavMeshAgent agent;
    private EnemySight enemySight;
    //private EnemyAttack enemyAttack;
    private EnemyAttackBox enemyAttackBox;
    private EnemyHurt enemyHurt;
    private Stats stats;

    private NavMeshObstacle obstacle;

    private GameManager gameManager;

    public enum currentStateEnum { idle = 0, walk = 1, attack = 2, hurt = 3, knockedDown = 4 };
    [Header("State Info")]
    public currentStateEnum currentState;

    [Header("Health Info")]
    public GameObject healthUI;
    public GameObject fillArea;
    public Image sliderBackground;

    [Header("Attacks & Sprites")]
    public GameObject spriteObject;
    public GameObject attackBox1;
    //public GameObject attackBox2;
    //public GameObject attackBox3;

    [Header("Checks")]
    public bool takingDamage;
    public bool knockedDown;

    [Header("Timers")]
    public float stunTime;
    public float knockdownTime;

    private Animator animator;
    // THIS VARIABLE HAUSES WHAT CURRENT NODE IS CYCLING
    private AnimatorStateInfo currentStateInfo;
    private static int currentAnimState;
    private static int idleState = Animator.StringToHash("Base Layer.fox_stand");
    private static int walkState = Animator.StringToHash("Base Layer.fox_walk");
    private static int attackState = Animator.StringToHash("Base Layer.fox_punch");
    private static int hurtState = Animator.StringToHash("Base Layer.fox_hurt");
    private static int fallState = Animator.StringToHash("Base Layer.fox_fox_knockedDown");
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemySight = GetComponent<EnemySight>();
        //enemyAttack = GetComponent<EnemyAttack>();
        enemyAttackBox = GetComponent<EnemyAttackBox>();
        enemyHurt = GetComponent<EnemyHurt>();
        stats = GetComponent<Stats>();
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();

        obstacle = GetComponent<NavMeshObstacle>();
    }

    private void Update()
    {
        //---knocked down---
        if (knockedDown == true && takingDamage == false)
        {
            agent.enabled = false;
            obstacle.enabled = true;
            stats.displayUI = true;
            healthUI = GameObject.FindGameObjectWithTag("EnemyHealthUI");
            //animator.SetBool("Knocked Down", true);
            StartCoroutine(KnockedDown());
        }
        //---take damage---
        else if (takingDamage == true)
        {
            agent.enabled = false;
            obstacle.enabled = true;
            //stats.displayUI = true;
            //healthUI = GameObject.FindGameObjectWithTag("EnemyHealthUI");
            //animator.SetBool("Is Hit", true);
            StartCoroutine(TookDamage());
        }
        //---attack---
        else if (takingDamage == false && enemySight.playerInSight &&
            enemySight.player.GetComponent<HorseController>().knockedDown == false
            && enemySight.targetDistance < enemyAttackBox.attackRange)
        /*&& enemySight.playerDistance < enemyAttack.attackRange )
        && navMeshAgent.velocity.sqrMagnitude < enemyAttack.attackStartDelay)*/
        {
            agent.enabled = false;
            obstacle.enabled = true;
            //stats.displayUI = true;
            //healthUI = GameObject.FindGameObjectWithTag("EnemyHealthUI");

            if (takingDamage)
            {
                animator.SetBool("Attack", false);
            }
            else
            {
                animator.SetBool("Attack", true);
                animator.SetBool("hasTarget", false);
                currentAnimState = attackState;
            }
        }
        //---follow---
        else if (enemySight.player.GetComponent<HorseController>().knockedDown == false &&
            takingDamage == false && enemySight.playerInSight && enemySight.targetDistance > enemyAttackBox.attackRange)
        {
            agent.enabled = true;
            obstacle.enabled = false;
            stats.displayUI = false;
            healthUI = null;
            animator.SetBool("hasTarget", true);
            animator.SetBool("Attack", false);
            currentAnimState = walkState;
        }
        //---idle---
        else if (takingDamage == false && enemySight.playerInSight == false)
        {
            agent.enabled = false;
            obstacle.enabled = true;
            //stats.displayUI = false;
            //healthUI = null;
            animator.SetBool("hasTarget", false);
            animator.SetBool("Attack", false);
        }

        if (currentAnimState == idleState)
        {
            // SET FLAG TO IDLE
            currentState = currentStateEnum.idle;
            stats.displayUI = false;
            healthUI = null;
            //if (stats.displayUI == true && GameObject.FindGameObjectWithTag("EnemyHealthUI"))
            //{
            //    healthUI = GameObject.FindGameObjectWithTag("EnemyHealthUI");
            //    healthUI.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
            //    sliderBackground = healthUI.gameObject.GetComponentInChildren<Image>();
            //    sliderBackground.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
                
            //}
        }
        else if (currentAnimState == walkState)
        {
            // SET FLAG TO WALK
            currentState = currentStateEnum.walk;
            stats.displayUI = false;
            healthUI = null;
            //if(stats.displayUI==true && GameObject.FindGameObjectWithTag("EnemyHealthUI"))
            //{
            //    healthUI = GameObject.FindGameObjectWithTag("EnemyHealthUI");
            //    healthUI.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
            //    //sliderBackground = healthUI.gameObject.GetComponentInChildren<Image>();
            //    //sliderBackground.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
            //}
        }
        else if (currentAnimState == attackState)
        {
            // SET FLAG TO ATTACK
            currentState = currentStateEnum.attack;
            stats.displayUI = true;
            healthUI = GameObject.FindGameObjectWithTag("EnemyHealthUI");
            //if (stats.displayUI == true && GameObject.FindGameObjectWithTag("EnemyHealthUI") && gameManager.enemyArray.Length>1)
            //{
            //    healthUI = GameObject.FindGameObjectWithTag("EnemyHealthUI");
            //    healthUI.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
            //    //sliderBackground = healthUI.gameObject.GetComponentInChildren<Image>();
            //    //sliderBackground.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
            //}
        }
        else if (currentAnimState == hurtState)
        {
            // SET FLAG TO HURT
            currentState = currentStateEnum.hurt;
            stats.displayUI = true;
            //if (stats.displayUI == true && GameObject.FindGameObjectWithTag("EnemyHealthUI"))
            //{
            //    healthUI = GameObject.FindGameObjectWithTag("EnemyHealthUI");
            //    healthUI.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
            //    sliderBackground = healthUI.gameObject.GetComponentInChildren<Image>();
            //    sliderBackground.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
            //}
        }
        // TO GET ANIMATOR STATE
        currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // TO CONVERT STRING INFO INTO INTEGER
        currentAnimState = currentStateInfo.fullPathHash;
    }

    private IEnumerator KnockedDown()
    {
        animator.Play("fox_knockedDown");
        knockedDown = true;

        yield return new WaitForSeconds(knockdownTime);

        animator.SetBool("Knocked Down", false);
        knockedDown = false;
    }

    private IEnumerator TookDamage()
    {
        //--NO LOOP        
        takingDamage = true;
        animator.SetBool("Is Hit", true);
        animator.SetBool("Attack", false);

        yield return new WaitForSeconds(stunTime);

        takingDamage = false;
        animator.SetBool("Is Hit", false);
    }
}
