using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBox : MonoBehaviour
{
    public GameObject attackBox1;// attackBox2, attackBox3;
    public float attackRange;

    public void Attack1()
    {
        attackBox1.gameObject.SetActive(true);
    }
    //public void Attack2()
    //{
    //    attackBox2.gameObject.SetActive(true);
    //}
    //public void Attack3()
    //{
    //    attackBox3.gameObject.SetActive(true);
    //}
    public void DeactivateAttackBox()
    {
        attackBox1.gameObject.SetActive(false);
        //attackBox2.gameObject.SetActive(false);
        //attackBox3.gameObject.SetActive(false);
    }
}
