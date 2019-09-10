using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Transform bar;
    private HealthSystem healthSystem;
    void Awake()
    {
        bar = transform.Find("Bar");        
    }

    //public void SetSize(float sizeNormalized)
    //{
    //    bar.localScale = new Vector3(sizeNormalized, 1f);
    //}
    public void SetColor(Color color)
    {
        bar.Find("BarSprite").GetComponent<SpriteRenderer>().color = color;
    }
    public void Setup(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;
        healthSystem.OnHealthChanged += HealthSyetem_OnHealthChanged;
    }
    private void HealthSyetem_OnHealthChanged(object sender, EventArgs e)
    {
        bar.localScale = new Vector3(healthSystem.GetHealthPercent(), 1f);
    }
    private void Update()
    {
        //bar.localScale = new Vector3(healthSystem.GetHealthPercent(), 1f);
    }
}
