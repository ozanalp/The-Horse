using UnityEngine;

public class CameraStop : MonoBehaviour
{
    public GameObject otherGameObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Camera Midpoint")
        {
            otherGameObject = other.transform.parent.gameObject;
            //otherGameObject.GetComponent<CameraController>().isFollowing = false;
            //isFollowing artık public static bool olduğu için uzun başlatmaya gerek yok
            CameraController.isFollowing = false;
            gameObject.SetActive(false);
        }
        else
        {
            return;
        }
    }
}
