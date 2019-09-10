using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWalk : MonoBehaviour
{
    public float enemySpeed;
    public float enemyCurrentSpeed;
    public float grabbableDistance;
    public bool facingRight;
    public GameObject spriteObject;
    [Header("Avoidance")]
    [SerializeField] private LayerMask raycastLayer;

    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private EnemySight enemySight;
    private EnemyState enemyState;
    //private Vector3 enemyRelativePosition;
    //private Grabbable grabbable;
    public float zPosDifference;

    private NavMeshObstacle obstacle;

    public float avoidEnemyOffsetZ;
    private bool grabbableAhead;
    private RaycastHit hitInfo;
    public float smoothing = 1f;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemySight = GetComponent<EnemySight>();
        enemyState = GetComponent<EnemyState>();
        animator = GetComponent<Animator>();
        //grabbable = GetComponent<Grabbable>();

        obstacle = GetComponent<NavMeshObstacle>();

        navMeshAgent.speed = enemySpeed;
    }

    private void Update()
    {
        RaycastDetection();

        //if (enemySight.playerInSight)
        //{
        //    if (enemySight.playerDistance <= navMeshAgent.stoppingDistance)
        //    {
        //        navMeshAgent.destination = enemySight.player.transform.position;
        //        navMeshAgent.isStopped = true;
        //    }
        //    else if (enemySight.playerDistance > navMeshAgent.stoppingDistance)
        //    {
        //        navMeshAgent.isStopped = false;
        //        navMeshAgent.SetDestination(enemySight.player.transform.position);
        //        navMeshAgent.updateRotation = false;
        //        animator.SetBool("hasTarget", true);
        //    }
        //}
        //else
        //{
        //    navMeshAgent.isStopped = true;
        //    animator.SetBool("hasTarget", false);
        //}
        //if (navMeshAgent.isStopped)
        //{
        //    animator.SetBool("hasTarget", false);
        //}
        if (enemyState.currentState == EnemyState.currentStateEnum.walk)
        {
            Walk();
        }
        else if (enemyState.currentState == EnemyState.currentStateEnum.idle)
        {
            Stop();
        }
    }

    private void Walk()
    {
        obstacle.enabled = false;
        navMeshAgent.enabled = true;

        if (!grabbableAhead)
        {
            if (enemySight.playerOnTheLeft && facingRight || !enemySight.playerOnTheLeft && !facingRight)
            {
                Flip();
            }
            navMeshAgent.speed = enemySpeed;
            enemyCurrentSpeed = navMeshAgent.velocity.sqrMagnitude;
            navMeshAgent.isStopped = false;
            navMeshAgent.updateRotation = false;
            navMeshAgent.SetDestination(enemySight.target.transform.position);
        }
        else
        {
            if (enemySight.playerOnTheLeft && facingRight || !enemySight.playerOnTheLeft && !facingRight)
            {
                Flip();
            }
            navMeshAgent.speed = enemySpeed;
            enemyCurrentSpeed = navMeshAgent.velocity.sqrMagnitude;
            navMeshAgent.isStopped = false;
            navMeshAgent.updateRotation = false;
            StartCoroutine(MoveAside(hitInfo));
            navMeshAgent.SetDestination(enemySight.target.transform.position);
        }

        animator.SetBool("hasTarget", true);
        animator.SetBool("Attack", false);
    }
    private void Stop()
    {
        obstacle.enabled = true;
        if (!obstacle.enabled)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 thisRotation = new Vector3(0, 180, 0);
        transform.eulerAngles += thisRotation;
    }

    private void RaycastDetection()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.right * -1;
        float maxDistance = 3f;

        Debug.DrawRay(origin, direction * maxDistance, Color.blue);

        Ray ray = new Ray(origin, direction * 1f);

        grabbableAhead = Physics.Raycast(ray, out hitInfo, maxDistance, raycastLayer);
    }

    private IEnumerator MoveAside(RaycastHit hitInfo)
    {

        Transform other = hitInfo.collider.GetComponent<Transform>();
        Debug.Log("Ray hit " + other.name);

        zPosDifference = other.position.z - transform.position.z;
        //zPosDifference = Mathf.Abs(other.position.z - transform.position.z);
        grabbableDistance = Vector3.Distance(other.position, transform.position);

        //while (zPosDifference < avoidEnemyOffsetZ)
        //{
        //    transform.position = Vector3.Lerp(transform.position, other.position + new Vector3(1, 0, 1), smoothing * Time.deltaTime);

        //    yield return null;
        //}

        if (zPosDifference < 0)
        {
            transform.Translate(Vector3.forward * -smoothing * Time.deltaTime);
        }
        else if (zPosDifference > 0)
        {
            transform.Translate(Vector3.forward * smoothing * Time.deltaTime);
        }
        yield return new WaitForSeconds(1f);
    }
}
