using System.Collections;
using UnityEngine;
using System;

public class HorseController : MonoBehaviour
{
    [Header("Boundaries")]
    public float xMin;
    public float Xmax;
    public float zMin;
    public float zMax;

    [Header("Force & Timers")]
    public float walkMovementSpeed;
    public float attackMovementSpeed;
    private float movementSpeed;
    


    public float knockbackForce;
    public float shotPower;
    public float knockedDownTime = 2f;
    public float stunTime;
    public float checkRadius;
    public LayerMask checkLayers;

    [Header("Checks")]
    public bool facingRight;
    public bool isBlocking;
    public bool canMove;
    public bool takingDamage;
    public bool knockedDown;

    private Rigidbody rb;
    private Animator animator;
    private AnimatorStateInfo currentStateInfo;
    private static int currentState;
    private static int idleState = Animator.StringToHash("Base Layer.horse_idle");
    private static int walkState = Animator.StringToHash("Base Layer.horse_walk");
    private static int runState = Animator.StringToHash("Base Layer.horse_run");
    private static int catchingState = Animator.StringToHash("Base Layer.horse_catching_fox");
    private static int humpingState = Animator.StringToHash("Base Layer.horse_humping_fox");
    private static int releaseState = Animator.StringToHash("Base Layer.horse_release_fox");

    [Header("Attacks & Sprites")]
    public GameObject attackBox1;
    public GameObject attackBox2;
    public GameObject attackBox3;
    public Transform shootPoint;
    public Sprite attackHitFrame1, attackHitFrame2, attackHitFrame3;
    [SerializeField] private GameObject shootPrf;
    private SpriteRenderer currentSprite;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentSprite = GetComponentInChildren<SpriteRenderer>();
        movementSpeed = walkMovementSpeed;
        facingRight = true;
        isBlocking = false;
        canMove = true;
    }

    private void Update()
    {
        // GETTING THE ANIMATOR STATE INFO IN BASE LAYER
        currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // CONVERTING THE STATE INFO INTO A HASH, INTEGER
        currentState = currentStateInfo.fullPathHash;
        // WE CAN COMPARE OUR ANIMATIONS NOW
        if (currentState == idleState)
        {

        }
        if (currentState == walkState)
        {

        }
        if (currentState == catchingState)
        {

        }
        if (currentState == humpingState)
        {

        }
        if (currentState == releaseState)
        {

        }

        //---speed control based on the states---
        if (currentState == idleState || currentState == walkState)
        {
            movementSpeed = walkMovementSpeed;
        }
        else if (currentState == runState)
        {
            movementSpeed = walkMovementSpeed * 2;
        }
        else
        {
            movementSpeed = attackMovementSpeed;            
        }
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);

        Grabbing grabbing = GetComponent<Grabbing>();
        bool grabOn = grabbing.grabbed;
        bool fReleased = grabbing.foxReleased;
        //---movement---
        if (!grabOn && fReleased && canMove)
        {
            if (currentState == runState || currentState == walkState)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    movementSpeed = walkMovementSpeed * 4;
                    animator.SetBool("Running", true);
                }
                else
                {
                    movementSpeed = walkMovementSpeed;
                    animator.SetBool("Running", false);
                }
            }            

            rb.isKinematic = false;
            rb.velocity = movement * movementSpeed;
            rb.position = new Vector3(
                Mathf.Clamp(rb.position.x, xMin, Xmax),
                transform.position.y,
                Mathf.Clamp(rb.position.z, zMin, zMax));

            if (moveHorizontal < 0 && facingRight && canMove || moveHorizontal > 0 && !facingRight && canMove)
            {
                Flip();
            }
            animator.SetFloat("Speed", rb.velocity.sqrMagnitude);

            //---combo state---
            if (Input.GetMouseButton(0))
            {
                animator.SetBool("Attack", true);
            }
            else
            {
                animator.SetBool("Attack", false);
            }
            // THE DISPLAYED SPRITE NUMBER EQUALS TO THE DESIRED HITTING FRAME SPRITE
            if (attackHitFrame1 == currentSprite.sprite)
            {
                attackBox1.gameObject.SetActive(true);
            }
            else if (attackHitFrame2 == currentSprite.sprite)
            {
                attackBox2.gameObject.SetActive(true);
            }
            else if (attackHitFrame3 == currentSprite.sprite)
            {
                attackBox3.gameObject.SetActive(true);
            }
            else
            {
                attackBox1.gameObject.SetActive(false);
                attackBox2.gameObject.SetActive(false);
                attackBox3.gameObject.SetActive(false);
            }

            //---block animation---
            if (Input.GetMouseButton(2))
            {
                rb.isKinematic = true;
                animator.SetBool("Block", true);
                isBlocking = true;
            }
            else
            {
                rb.isKinematic = false;
                animator.SetBool("Block", false);
                isBlocking = false;
            }

            //---hit test---
            //--COLLISION DETECTION WILL BE IMPLEMENTED
            if (takingDamage == true && knockedDown == false)
            {
                rb.isKinematic = true;
                StartCoroutine(TookDamage());
            }

            //---knoc down test---
            if (knockedDown == true)
            {
                rb.isKinematic = true;
                StartCoroutine(KnockedDown());
            }

            //---projectile test---
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject shot = Instantiate(shootPrf, shootPoint.position, Quaternion.Euler(0, 0, 270), transform.parent);
                shot.GetComponent<Rigidbody>().AddForce(transform.right * shotPower);
                Destroy(shot, 5f);
            }
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 thisRotation = new Vector3(0, 180, 0);
        transform.eulerAngles += thisRotation;
    }

    private IEnumerator TookDamage()
    {
        animator.Play("horse_hurt");
        canMove = false;

        yield return new WaitForSeconds(stunTime);

        canMove = true;
        takingDamage = false;
    }

    private IEnumerator KnockedDown()
    {
        animator.Play("horse_fall");
        animator.SetBool("Knocked Down", true); //--NO LOOP
        canMove = false;

        rb.AddForce(transform.right * knockbackForce);

        yield return new WaitForSeconds(knockedDownTime);

        animator.SetBool("Knocked Down", false);
        canMove = true;
        knockedDown = false;
    }
}
