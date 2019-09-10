using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private Image healthUI;

    private void Awake()
    {
        healthUI = GameObject.FindWithTag(Tags.HEALTH_UI).GetComponent<Image>();
    }

    public void DisplayHealth(float value)
    {
        value /= 100;

        if (value < 0)
        {
            value = 0f;
        }

        healthUI.fillAmount = value;
    }
}
