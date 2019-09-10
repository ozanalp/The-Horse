using System.Collections;
using UnityEngine;

public class AnimationDelegate : MonoBehaviour
{
    public GameObject leftHand, rightHand, leftFoot, rightFoot;

    public float standUpTimer = 2f;
    private CharacterAnimation animationScript;

    private AudioSource audioSource;
    [SerializeField] private AudioClip whooshSound, fallSound, groundHitSound, deadSound;
    [SerializeField] private AudioClip[] punchSound;
    private AudioClip punchClip;

    private EnemyController enemy;

    private CameraShake cameraShake;

    private void Awake()
    {
        animationScript = GetComponent<CharacterAnimation>();
        audioSource = GetComponent<AudioSource>();

        if (gameObject.CompareTag(Tags.ENEMY_TAG))
        {
            enemy = GetComponentInParent<EnemyController>();
        }

        cameraShake = GameObject.FindGameObjectWithTag(Tags.MAIN_CAMERA_TAG).GetComponent<CameraShake>();
    }

    private void LeftHandAttackOn()
    {
        leftHand.SetActive(true);
    }

    private void LeftHandAttackOff()
    {
        if (leftHand.activeInHierarchy)
        {
            leftHand.SetActive(false);
        }
    }

    private void LeftFootAttackOn()
    {
        leftFoot.SetActive(true);
    }

    private void LeftFootAttackOff()
    {
        if (leftFoot.activeInHierarchy)
        {
            leftFoot.SetActive(false);
        }
    }

    private void RightHandAttackOn()
    {
        rightHand.SetActive(true);
    }

    private void RightHandAttackOff()
    {
        if (rightHand.activeInHierarchy)
        {
            rightHand.SetActive(false);
        }
    }

    private void RightFootAttackOn()
    {
        rightFoot.SetActive(true);
    }

    private void RightFootAttackOff()
    {
        if (rightFoot.activeInHierarchy)
        {
            rightFoot.SetActive(false);
        }
    }

    // hemen aşağıdaki 4 fonksiyon ile belirli hareketlerle Animation Event ekleyip belirli animasyonları
    // aynı gameObject.i kullanarak çağırabileceğiz
    private void TagLeftArm()
    {
        leftHand.tag = Tags.LEFT_ARM_TAG;
    }

    private void UnTagLeftArm()
    {
        leftHand.tag = Tags.UNTAGGED_TAG;
    }

    private void TagLeftFoot()
    {
        leftFoot.tag = Tags.LEFT_LEG_TAG;
    }

    private void UnTagLeftFoot()
    {
        leftFoot.tag = Tags.UNTAGGED_TAG;
    }

    private void EnemyStandUp()
    {
        StartCoroutine(StandUpAfterTime());
    }

    private IEnumerator StandUpAfterTime()
    {
        yield return new WaitForSeconds(standUpTimer);
        animationScript.StandUP();
    }

    public void AttackSound()
    {
        audioSource.volume = .2f;
        audioSource.clip = whooshSound;
        audioSource.Play();
    }

    public void PunchSound()
    {
        audioSource.volume = .3f;
        int index = Random.Range(0, punchSound.Length);
        punchClip = punchSound[index];
        audioSource.clip = punchClip;
        audioSource.Play();
    }

    private void CharacterDiedSound()
    {
        audioSource.volume = 1f;
        audioSource.clip = deadSound;
        audioSource.Play();
    }

    private void EnemyKnockdownSound()
    {
        audioSource.clip = fallSound;
        audioSource.Play();
    }

    private void EnemyHitGroundSound()
    {
        audioSource.clip = groundHitSound;
        audioSource.Play();
    }

    private void DisableMovement()
    {
        enemy.enabled = false;

        // set the enemy layer to default
        //transform.parent.gameObject.layer = 0;
    }

    private void EnableMovement()
    {
        enemy.enabled = true;

        // set the enemy layer back to enemy 
        //transform.parent.gameObject.layer = 8;
    }

    private void CameraShakeOnFall()
    {
        cameraShake.ShouldShake = true;
    }

    void CharacterDied()
    {
        Invoke("DeactivateGameObject", 2);
    }

    void DeactivateGameObject()
    {
        EnemyManager.instance.SpawnEnemy();

        Destroy(transform.parent.gameObject);
    }
}
