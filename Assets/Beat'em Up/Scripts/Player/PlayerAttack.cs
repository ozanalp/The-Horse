using System.Collections;
using UnityEngine;

public enum ComboState
{
    NONE, PUNCH_1, PUNCH_2, PUNCH_3, KICK_1, KICK_2
}

public class PlayerAttack : MonoBehaviour
{
    // ANIMATION
    private CharacterAnimation playerAnimation;

    // COMBO ATTACK
    private ComboState currentComboState;
    private bool activateTimerToReset;
    private float defaultComboTimer = .4f;
    private float currentComboTimer;

    public bool canAttack;

    private void Awake()
    {
        playerAnimation = GetComponentInChildren<CharacterAnimation>();
    }

    private void Start()
    {
        currentComboTimer = defaultComboTimer;
        currentComboState = ComboState.NONE;
        canAttack = true;
    }

    private void Update()
    {
        ComboAttack();
        ResetComboState();
    }

    private void ComboAttack()
    {
        if (canAttack)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (currentComboState == ComboState.PUNCH_3 ||
                    currentComboState == ComboState.KICK_1 ||
                    currentComboState == ComboState.KICK_2)
                {
                    return;
                }

                currentComboState++;
                activateTimerToReset = true;
                currentComboTimer = defaultComboTimer;

                if (currentComboState == ComboState.PUNCH_1)
                {
                    playerAnimation.Punch_1();
                }
                if (currentComboState == ComboState.PUNCH_2)
                {
                    playerAnimation.Punch_2();
                }
                if (currentComboState == ComboState.PUNCH_3)
                {
                    playerAnimation.Punch_3();
                }
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (currentComboState == ComboState.PUNCH_3 ||
                    currentComboState == ComboState.KICK_2)
                {
                    return;
                }

                if (currentComboState == ComboState.NONE ||
                    currentComboState == ComboState.PUNCH_1 ||
                    currentComboState == ComboState.PUNCH_2)
                {
                    currentComboState = ComboState.KICK_1;
                }
                else if (currentComboState == ComboState.KICK_1)
                {
                    currentComboState++;
                }

                activateTimerToReset = true;
                currentComboTimer = defaultComboTimer;

                if (currentComboState == ComboState.KICK_1)
                {
                    playerAnimation.Kick_1();
                }
                if (currentComboState == ComboState.KICK_2)
                {
                    playerAnimation.Kick_2();
                }
            }
        }
        else return;      
    }

    private void ResetComboState()
    {
        if (activateTimerToReset)
        {
            currentComboTimer -= Time.deltaTime;

            if (currentComboTimer <= 0)
            {
                currentComboState = ComboState.NONE;
                activateTimerToReset = false;
                currentComboTimer = defaultComboTimer;
            }
        }
    }
}
