using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AttackType { lPunch = 0, hPunch = 1, lKick = 2, hKick = 3, movement = 4 };
public class ComboAttack : MonoBehaviour
{
    [Header("Inputs")] //OYUNCUNUN SALDIRI YAPMAK İÇİN BASACAĞI TUŞLAR
    public KeyCode _lowPunch;
    public KeyCode _highPunch;
    public KeyCode _lowKick;
    public KeyCode _highKick;

    [Header("Attacks")]
    public Attack lowPunch;
    public Attack highPunch;
    public Attack lowKick;
    public Attack highKick;
    public List<Combo> combos;
    public float comboLeeway = 0.25f;

    [Header("Components")]
    private Animator anima;

    private Attack currentAttack = null;
    private ComboInput lastInput = null;
    private readonly List<int> currentCombos = new List<int>();

    private float timer = 0f;
    private float leeway = 0;
    private bool skip = false;

    private void Start()
    {
        anima = GetComponent<Animator>();
        PrimeCombos();
    }

    public void PrimeCombos()
    {
        for (int i = 0; i < combos.Count; i++)
        {
            Combo c = combos[i];
            c.onInput.AddListener(() =>
            {
                //CALL ATTACK FUNCTION WITH THE COMBO'S ATTACK 
                skip = true;
                lastInput = null;
                Attack(c.comboAttack);
                ResetCombos();
            });
        }
    }

    private void Update()
    {
        Grabbing grabbing = GetComponentInParent<Grabbing>();
        bool grabOn = grabbing.grabbed;
        if (!grabOn)
        {
            if (currentAttack != null)
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    currentAttack = null;
                }
                return;
            }

            if (currentCombos.Count > 0)
            {
                leeway += Time.deltaTime;
                if (leeway >= comboLeeway)
                {
                    if (lastInput != null)
                    {
                        Attack attack = getAttackFromType(lastInput.type);

                        if (attack != null)
                        {
                            Attack(attack);
                        }

                        lastInput = null;
                    }
                    ResetCombos();
                }
            }
            else
            {
                leeway = 0;
            }

            #region input check start
            ComboInput input = null;
            if (Input.GetKeyDown(_lowPunch))
            {
                input = new ComboInput(AttackType.lPunch);
            }
            if (Input.GetKeyDown(_highPunch))
            {
                input = new ComboInput(AttackType.hPunch);
            }
            if (Input.GetKeyDown(_lowKick))
            {
                input = new ComboInput(AttackType.lKick);
            }
            if (Input.GetKeyDown(_highKick))
            {
                input = new ComboInput(AttackType.hKick);
            }

            Vector2 movement = Vector2.zero;
            if (ComboMovement.InputDownX())
            {
                movement.x = ComboMovement.x;
            }
            if (ComboMovement.InputDownY())
            {
                movement.y = ComboMovement.y;
            }
            if (movement != Vector2.zero)
            {
                input = new ComboInput(movement);
            }

            if (input == null)
            {
                return;
            }
            lastInput = input;
            #endregion

            List<int> remove = new List<int>();
            for (int i = 0; i < currentCombos.Count; i++)
            {
                Combo c = combos[currentCombos[i]];
                if (c.continueCombo(input))
                {
                    leeway = 0;
                }
                else
                {
                    remove.Add(i);
                }
            }

            if (skip)
            {
                skip = false;
                return;
            }

            for (int i = 0; i < combos.Count; i++)
            {
                if (currentCombos.Contains(i))
                {
                    continue;
                }

                if (combos[i].continueCombo(input))
                {
                    currentCombos.Add(i);
                    leeway = 0;
                }
            }

            foreach (int i in remove)
            {
                currentCombos.RemoveAt(i);
            }

            Attack att = getAttackFromType(input.type);
            if (att != null && currentCombos.Count <= 0)
            {
                Attack(att);
            }
        }
    }

    private void ResetCombos()
    {
        leeway = 0;
        for (int i = 0; i < currentCombos.Count; i++)
        {
            Combo c = combos[currentCombos[i]];
            c.ResetCombo();
        }
        currentCombos.Clear();
    }

    private void Attack(Attack attack)
    {
        currentAttack = attack;
        timer = attack.animaLength;
        anima.Play(attack.animaName, -1, 0);
    }

    private Attack getAttackFromType(AttackType t)
    {
        if (t == AttackType.lPunch)
        {
            return lowPunch;
        }

        if (t == AttackType.hPunch)
        {
            return highPunch;
        }

        if (t == AttackType.lKick)
        {
            return lowKick;
        }

        if (t == AttackType.hKick)
        {
            return highKick;
        }

        return null;
    }
}

[System.Serializable]
public class Attack
{
    public string animaName;
    public float animaLength;
}

[System.Serializable]
public class ComboInput //HOLD THE INFO WHAT WE SHOULD PRESS TO INITIATE THAT COMBO ATTACK
{
    public AttackType type;
    public Vector2 movement;

    public ComboInput(AttackType t)
    {
        type = t;
        movement = Vector2.zero;
    }
    public ComboInput(Vector2 m)
    {
        type = AttackType.movement;
        movement = m;
    }

    //IF YOU ADD MORE VARIABLES MAKE SURE YOU PUT THEM INTO THE ISSAMEAS
    public bool isSameAs(ComboInput test)
    {
        return (type == AttackType.movement) ? (validMovement(test.movement)) : (type == test.type);
    }

    private bool validMovement(Vector2 move)
    {
        bool valid = true;
        if (movement.x != 0 && movement.x != move.x)
        {
            valid = false;
        }
        if (movement.y != 0 && movement.y != move.y)
        {
            valid = false;
        }
        return valid;
    }

}

[System.Serializable]
public class Combo
{
    public string name;
    public List<ComboInput> inputs;
    public Attack comboAttack;
    public UnityEvent onInput;
    private int currentInput = 0;

    public bool continueCombo(ComboInput i)
    {
        if (inputs[currentInput].isSameAs(i))
        {
            currentInput++;

            if (currentInput >= inputs.Count) //FINISHED THE INPUTS AND WE SHOULD DO THE ATTACK
            {
                onInput.Invoke();
                currentInput = 0;
            }
            return true;
        }
        else
        {
            currentInput = 0;
            return false;
        }
    }

    public ComboInput currentComboInput()
    {
        if (currentInput >= inputs.Count)
        {
            return null;
        }

        return inputs[currentInput];
    }

    public void ResetCombo()
    {
        currentInput = 0;
    }
}