using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Grabbing : MonoBehaviour
{
    //public GameObject holdPoint;
    //public float distance = 2f;
    //public Transform holdPoint;
    private Animator animator;
    public GameObject caughtFur;
    private Grabbable grabbable;
    private EnemySight enemySight;
    public Transform holdPoint;
    public Transform releasePoint;
    public float compareDistance;
    public float distance;
    [Header("Checks")]
    public bool grabbed = false;
    public bool closeEnough = false;
    public bool foxReleased = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        caughtFur = null;
    }

    private void Update()
    {
        CompareDistance();

        if (Input.GetKeyDown(KeyCode.LeftShift) && grabbable != null && closeEnough)
        {
            grabbed = true;
            foxReleased = false;
            //StartCoroutine(CatchingAnimation());
            CatchingFox();

            //grab on to the object
            //catchPoint.gameObject.transform.position = holdPoint.position;
            ////if your box has a rigidbody on it,and you want to take direct control of it
            ////you will want to set the rigidbody iskinematic to true.
            //catchPoint.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        }
        //else if (grabbed == true && Input.GetKeyDown(KeyCode.Z))             
        //{
        //    //GetKeyUp(KeyCode.LeftShift) sol shift tuşunu bırakınca kutuyu bırakır
        //    grabbed = false;
        //    catchPoint.gameObject.transform.parent = null;
        //    catchPoint.GetComponent<Rigidbody2D>().isKinematic = false;
        //}       

        // this implementation should be relatively the same as you 
        // already pick the object up.
        if (grabbed == true)
        {
            // do the grabby stuff

            //yeni animasyon gelmesi sebebiyle aşağıdaki iki satır çıkartılmıştır
            ////hit.collider.gameObject.transform.position = holdPoint.position;
            //catchPoint.transform.position = holdPoint.position;
            //catchPoint.transform.parent = transform;
            //holdPoint.transform.rotation = gameObject.transform.rotation;

            if (Input.GetKey(KeyCode.Z))
            {
                //yeni animasyon gelmesi sebebiyle aşağıdaki iki satır çıkartılmıştır
                //grabbed = false;
                //catchPoint.gameObject.transform.parent = null;
                //catchPoint.GetComponent<Rigidbody2D>().isKinematic = false;
                animator.Play("horse_humping_fox", -1, 0f);
            }
            if (Input.GetKey(KeyCode.X))
            {
                //instantiate fox at spawn point
                animator.SetTrigger("hasReleased");
                grabbed = false;
            }
        }
        // this implementation should be the opposite of that
        // i.e. setting the touched parent to null and placing it back on the ground
        else
        {
            // do the droppy stuff
        }
    }

    private IEnumerator CatchingAnimation()
    {
        CatchingFox();
        yield return new WaitForSeconds(0);

        //yeni animasyon gelmesi sebebiyle aşağıdaki iki satır çıkartılmıştır
        //catchPoint.gameObject.transform.position = holdPoint.position;
        ////if your box has a rigidbody on it,and you want to take direct control of it
        ////you will want to set the rigidbody iskinematic to true.
        //catchPoint.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    private void OnTriggerEnter(Collider collided)
    {
        caughtFur = collided.gameObject;
        grabbable = caughtFur.GetComponentInParent<Grabbable>();
    }
    private void OnTriggerExit(Collider collided)
    {
        caughtFur = null;
        grabbable = null;
        closeEnough = false;
    }
    public void CatchingFox()
    {
        animator.Play("horse_catching_fox");
        grabbable.gameObject.SetActive(false);
    }
    public void ReleaseFox()
    {
        grabbable.gameObject.SetActive(true);
        grabbable.transform.position = releasePoint.position;
        foxReleased = true;
        closeEnough = false;
    }

    private void CompareDistance()
    {
        if (!caughtFur)
        {
            return;
        }
        else
        {
            distance = Mathf.Abs(Vector3.Distance(holdPoint.position, caughtFur.transform.position));
            if (!caughtFur.GetComponentInParent<NavMeshAgent>())
            {
                return;
            }
            else
            {
                compareDistance = caughtFur.GetComponentInParent<NavMeshAgent>().stoppingDistance * 2;

                if (distance <= compareDistance)
                {
                    closeEnough = true;
                }
                else if (!caughtFur)
                {
                    closeEnough = false;
                }
            }
        }
    }
}
