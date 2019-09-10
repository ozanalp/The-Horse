using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCanAttack : MonoBehaviour
{
    PlayerAttack playerAttack;

    void Start()
    {
        playerAttack = FindObjectOfType<PlayerAttack>();
    }

    void CanAttack()
    {
        playerAttack.canAttack = true;
        Debug.Log("can");
    }
    void CantAttack()
    {
        playerAttack.canAttack = false;
        Debug.Log("can't");
    }
}
