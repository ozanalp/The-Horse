using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private CharacterAnimation enemyAnimation;
    private Rigidbody myBody;
    public float speed = 2f;
    private Transform playerTarget;
    public float attackDistance = 1f;
    public float chasePlayerAfterAttack = 1f;
    private float currentAttackTime;
    private float defaultAttackTime = 2f;

    public bool followPlayer, attackPlayer;

    private void Awake()
    {
        enemyAnimation = GetComponentInChildren<CharacterAnimation>();
        myBody = GetComponent<Rigidbody>();

        playerTarget = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;
    }

    private void Start()
    {
        followPlayer = true;
        currentAttackTime = defaultAttackTime;
    }

    private void FixedUpdate()
    {
        FollowPlayer();
    }

    private void Update()
    {
        Attack();
    }

    private void FollowPlayer()
    {
        if (!followPlayer)
        {
            return;
        }

        if (Vector3.Distance(transform.position, playerTarget.position) > attackDistance)
        {
            transform.LookAt(playerTarget);
            myBody.velocity = transform.forward * speed;

            if (myBody.velocity.sqrMagnitude != 0)
            {
                enemyAnimation.Walk(true);
            }
        }
        else if (Vector3.Distance(transform.position, playerTarget.position) <= attackDistance)
        {
            myBody.velocity = Vector3.zero;
            enemyAnimation.Walk(false);

            followPlayer = false;
            attackPlayer = true;
        }
    }

    private void Attack()
    {
        if (!attackPlayer)
        {
            return;
        }

        currentAttackTime += Time.deltaTime;
        transform.LookAt(playerTarget);

        if (currentAttackTime > defaultAttackTime)
        {
            enemyAnimation.EnemyAttack(Random.Range(0, 3));

            currentAttackTime = 0f;
        }
        // player ile enemy arasında kaçmaya yetecek kadar mesefa bırakmak için
        if (Vector3.Distance(transform.position, playerTarget.position) > attackDistance + chasePlayerAfterAttack)
        {
            attackPlayer = false;
            followPlayer = true;
        }
    }
}
