using System.Collections.Generic;
using UnityEngine;

public class HealthBarController3DCollage : MonoBehaviour
{
    [SerializeField] private HealthBar3DCollage healthBarPrf;

    private Dictionary<Health3DCollage, HealthBar3DCollage> healthBars = new Dictionary<Health3DCollage, HealthBar3DCollage>();

    private void Awake()
    {
        Health3DCollage.OnHealthAdded += AddHealthBar;
        Health3DCollage.OnHealthRemoved += RemoveHealthBar;
    }

    private void AddHealthBar(Health3DCollage health)
    {
        if (healthBars.ContainsKey(health) == false)
        {
            HealthBar3DCollage healthBar = Instantiate(healthBarPrf, transform);
            healthBars.Add(health, healthBar);
            healthBar.SetHealth(health);
        }
    }

    private void RemoveHealthBar(Health3DCollage health)
    {
        if (healthBars.ContainsKey(health))
        {
            Destroy(healthBars[health].gameObject);
            healthBars.Remove(health);
        }
    }
}
