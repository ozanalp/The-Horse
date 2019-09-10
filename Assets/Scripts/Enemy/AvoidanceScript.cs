using UnityEngine;

public class AvoidanceScript : MonoBehaviour
{
    public bool rightSensorBool;
    public bool centerSensorBool;
    public bool leftSensorBool;

    public int turnValue;
    public float turnSpeed = .001f;

    public float sensorLength;
    private float minDist = Mathf.Infinity;
    private Transform targetRotation;
    private Collider myCollider;

    public float yRotation;

    private void Update()
    {
        SensorController();
        TurnController();
    }

    private void TurnValueInterpriter()
    {

    }

    private void TurnController()
    {
        //Vector3 turnVector = new Vector3(0, turnValue, 0);
        Vector3 dir = new Vector3();

        if (rightSensorBool == true && centerSensorBool == false && leftSensorBool == false)
        {
            //turnValue = -45;
            dir = -Vector3.forward - Vector3.right;
        }
        else if (rightSensorBool == false && centerSensorBool == false && leftSensorBool == true)
        {
            //turnValue = 45;
            dir = Vector3.forward - Vector3.right;
        }
        else if (rightSensorBool == true && centerSensorBool == true && leftSensorBool == false)
        {
            //turnValue = -90;
            dir = (-Vector3.forward - Vector3.right) * 1.5f;
        }
        else if (rightSensorBool == false && centerSensorBool == true && leftSensorBool == true)
        {
            //turnValue = 90;
            dir = (Vector3.forward - Vector3.right) * 1.5f;
        }
        else if (rightSensorBool == true && centerSensorBool == true && leftSensorBool == true)
        {
            //turnValue = 180;
            dir = Vector3.right;
        }
        else
        {
            //turnValue = 0;
            dir = -Vector3.right;
        }
        //transform.Rotate(turnVector * Time.deltaTime * turnSpeed, Space.Self);
        transform.position = GetComponent<Rigidbody>().position + dir * turnSpeed * Time.deltaTime;
    }

    private void SensorController()
    {
        RightSensor();
        CentorSensor();
        LeftSensor();
    }

    private void RightSensor()
    {

        if (Physics.Raycast(transform.position, (Vector3.forward - Vector3.right).normalized, out RaycastHit hit, sensorLength))
        {
            if (hit.collider.tag != "Grabbable")
            {
                return;
            }
            else
            {
                rightSensorBool = true;
            }
        }
        else
        {
            rightSensorBool = false;
        }
    }

    private void LeftSensor()
    {

        if (Physics.Raycast(transform.position, (-Vector3.forward - Vector3.right).normalized, out RaycastHit hit, sensorLength))
        {
            if (hit.collider.tag != "Grabbable")
            {
                return;
            }
            else
            {
                leftSensorBool = true;
            }
        }
        else
        {
            leftSensorBool = false;
        }
    }

    private void CentorSensor()
    {

        if (Physics.Raycast(transform.position, -Vector3.right, out RaycastHit hit, sensorLength))
        {
            if (hit.collider.tag != "Grabbable")
            {
                return;
            }
            else
            {
                centerSensorBool = true;
            }
        }
        else
        {
            centerSensorBool = false;
        }
    }

    private void OnDrawGizmos()
    {  
        //CENTER
        Gizmos.DrawRay(transform.position, -Vector3.right * sensorLength);
        //LEFT
        Gizmos.DrawRay(transform.position, (-Vector3.forward - Vector3.right).normalized * sensorLength);
        //RIGHT
        Gizmos.DrawRay(transform.position, (Vector3.forward - Vector3.right).normalized * sensorLength);
    }
}