using Pathfinding;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{
    // What to chase?
    public Transform target;
    // How many times each second we will update our path
    public float updateRate = 2f;
    // Caching
    private Seeker seeker;
    //The calculated path
    public Path path;
    //The AI’s speed per second
    public float speed = 1f;
    //public ForceMode2D fMode;
    public bool pathIsEnded = false;
    // The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;
    // The waypoint we are currently moving towards
    private int currentWaypoint = 0;
    private bool searchingForPlayer = false;
    private Rigidbody rb;
    // Does enemy have a sight on the player
    private EnemySight enemySight;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody>();
        enemySight = GetComponent<EnemySight>();

        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }
        //StartCoroutine(UpdatePath());
        //seeker.StartPath(transform.position, target.position, OnPathComplete);
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    private IEnumerator SearchForPlayer()
    {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if (sResult == null)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SearchForPlayer());
        }
        else
        {

            target = sResult.transform;
            searchingForPlayer = false;
            //StartCoroutine(UpdatePath());
            InvokeRepeating("UpdatePath", 0f, .5f);

        }
    }

    private void UpdatePath()
    {

        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }

    }

    //private IEnumerator UpdatePath()
    //{
    //    if (target == null)
    //    {
    //        if (!searchingForPlayer)
    //        {
    //            searchingForPlayer = true;
    //            StartCoroutine(SearchForPlayer());
    //        }
    //        yield return false;
    //    }
    //    else
    //    {
    //        seeker.StartPath(transform.position, target.position, OnPathComplete);
    //        yield return new WaitForSeconds(1f / updateRate);
    //        StartCoroutine(UpdatePath());
    //    }
    //}
    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            // Set current path equal to p, the newly generated path
            path = p;
            // Reset current path
            currentWaypoint = 0;
        }
    }
    public void OnDisable()
    {
        if (GameObject.FindGameObjectWithTag("Grabbable").activeInHierarchy)
            seeker.pathCallback -= OnPathComplete;
        else return;
    }
    private void FixedUpdate()
    {
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }
        //Always look at player
        if (transform.position.x < target.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (path == null)
        {
            return;
        }
        // Checking if we reached end of the path or still have waypoints to go
        if (currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
            {
                return;
            }
            // When reach the end we gave it players new position.
            StartCoroutine(SearchForPlayer());
            pathIsEnded = true;
            return;
        }
        else
        {
            pathIsEnded = false;
        }

        float distanceToWaypoint;
        while (true)
        {
            // If you want maximum performance you can check the squared distance instead to get rid of a
            // square root calculation. But that is outside the scope of this tutorial.
            distanceToWaypoint = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                // Check if there is another waypoint or if we have reached the end of the path
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    pathIsEnded = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }
        // Slow down smoothly upon approaching the end of the path
        // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
        float speedFactor = pathIsEnded ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

        // Direction to the next waypoint
        // Normalize it so that it has a length of 1 world unit
        Vector3 dir = (path.vectorPath[currentWaypoint] - rb.position).normalized;
        // Multiply the direction by our desired speed to get a velocity
        if (enemySight.playerInSight)
        {
            Vector3 velocity = dir * speed * speedFactor;

            // Move the agent using the CharacterController component
            // Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
            if (!pathIsEnded)
            {
                transform.position = rb.position + velocity * Time.deltaTime;
            }
        }       
    }
}