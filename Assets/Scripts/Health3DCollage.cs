using System;
using UnityEngine;

public class Health3DCollage : MonoBehaviour
{
    public static event Action<Health3DCollage> OnHealthAdded = delegate { };
    public static event Action<Health3DCollage> OnHealthRemoved = delegate { };

    [SerializeField] private int maxHealth = 100;

    public int CurrentHealth { get; private set; }

    public event Action<float> OnHealthPctChanged = delegate { };

    private void OnEnable()
    {
        CurrentHealth = maxHealth;
        OnHealthAdded(this);
    }
    public void ModifyHealth(int amount)
    {
        CurrentHealth += amount;

        float currentHealthPct = (float)CurrentHealth / maxHealth;
        OnHealthPctChanged(currentHealthPct);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ModifyHealth(-10);
        }
    }
    private void OnDisable()
    {
        OnHealthRemoved(this);
    }
}
