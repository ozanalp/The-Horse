using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static bool isFollowing;
    public float cameraSpeed;

    private Vector3 playerXPos;
    private GameObject playerTarget;
    private void Awake()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    private void Update()
    {
        playerXPos = new Vector3(playerTarget.transform.position.x, 0, 0);

        if (isFollowing)
        {
            transform.position = Vector3.Lerp(transform.position, playerXPos, cameraSpeed);
        }
    }
}
