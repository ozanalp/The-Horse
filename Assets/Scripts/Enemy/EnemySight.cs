using System;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public GameObject player;
    [Header("Checks")]
    public bool grabbableInSight;
    public bool playerInSight;
    public bool playerOnTheLeft;

    [Header("Target Info")]
    public GameObject target;
    public float targetDistance;
    public float playerDistance;

    private Vector3 playerRelativePosition;
    private GameObject frontTarget;
    private GameObject backTarget;
    private float frontTargetDistance;
    private float backtargetDitance;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        frontTarget = GameObject.Find("Enemy Front Target");
        backTarget = GameObject.Find("Enemy Back Target");
    }

    private void Update()
    {
        playerRelativePosition = player.transform.position - transform.position;
        if (playerRelativePosition.x < 0)
        {
            playerOnTheLeft = true;
        }
        else if (playerRelativePosition.x > 0)
        {
            playerOnTheLeft = false;
        }

        frontTargetDistance = (float)Math.Floor(Vector3.Distance(frontTarget.transform.position, gameObject.transform.position) * 100) / 100;
        backtargetDitance = (float)Math.Floor(Vector3.Distance(backTarget.transform.position, gameObject.transform.position) * 100) / 100;

        if (frontTargetDistance < backtargetDitance)
        {
            target = frontTarget;
        }
        else if (frontTargetDistance > backtargetDitance)
        {
            target = backTarget;
        }
        else if (playerDistance < targetDistance )
        {
            target = player;
        }

        //playerDistance = (float)Math.Round( Vector3.Distance(transform.position, player.transform.position),1);
        // 2 DECIMAL PLACES RESULT, 1000 GIVES 3 DECIMAL PLACES
        targetDistance = (float)Math.Floor(Vector3.Distance(transform.position, target.transform.position) * 100) / 100;
        playerDistance = (float)Math.Floor(Vector3.Distance(player.transform.position, gameObject.transform.position) * 100) / 100;
        // NO DECIMAL PLACES
        //playerDistance = (float)Math.Truncate(Vector3.Distance(transform.position, player.transform.position));
    }
    //AS LONG AS THE PLAYER STAYS IN THE SPHERE
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInSight = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInSight = false;
        }
    }
}
