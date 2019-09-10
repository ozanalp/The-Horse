using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar3DCollage : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private float updateSpeedSeconds = 0.5f;
    [SerializeField] private float positionOffset;

    private Health3DCollage health;

    public void SetHealth(Health3DCollage health)
    {
        this.health = health;
        health.OnHealthPctChanged += HandleHealthChanged;
    }

    private void HandleHealthChanged(float pct)
    {
        StartCoroutine(ChangeToPct(pct));
    }

    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = healthBar.value;
        float elapsed = 0;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            healthBar.value = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }

        healthBar.value = pct;
        Debug.Log(pct);
    }

    private void LateUpdate()
    {
        //transform.position = Camera.main.WorldToScreenPoint(health.transform.position + Vector3.up * positionOffset);
        transform.position = Camera.main.WorldToScreenPoint(Vector3.up * positionOffset);
    }

    private void OnDestroy()
    {
        health.OnHealthPctChanged -= HandleHealthChanged;
    }
}
