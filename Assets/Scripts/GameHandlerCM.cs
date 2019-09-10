using CodeMonkey.Utils;
using System;
using UnityEngine;
using CodeMonkey;

public class GameHandlerCM : MonoBehaviour
{
    public Transform prfHealthBar;
    public Transform player;

    private void Start()
    {
        HealthSystem healthSystem = new HealthSystem(100);

        prfHealthBar.position = player.position + new Vector3(0, 2, 0);
        Transform healthBarTransform = Instantiate(prfHealthBar, prfHealthBar.position, Quaternion.identity, player);
        HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
        healthBar.Setup(healthSystem);

        //CMDebug.ButtonUI(new Vector2(40, 30), "damage", () =>
        //{
        //    healthSystem.Damage(10);
        //    Debug.Log("Damaged: " + healthSystem.GetHealth());
        //});

        //CMDebug.ButtonUI(new Vector2(-40, 30), "heal", () =>
        //{
        //    healthSystem.Heal(10);
        //    Debug.Log("Healed: " + healthSystem.GetHealth());
        //});
        if (healthSystem.GetHealth() < 30f)
        {
            if ((healthSystem.GetHealthPercent() % 3) == 0)
            {
                healthBar.SetColor(Color.black);
            }
            else
            {
                healthBar.SetColor(Color.yellow);
            }
        }

        //float health = 1f;
        //healthBar.SetColor(Color.yellow);
        //FunctionPeriodic.Create(() =>
        //{
        //    if (health > .01f)
        //    {
        //        health -= .01f;
        //        healthBar.SetSize(health);

        //        if (health < .3f)
        //        {
        //            if (Math.Truncate(health * 100 % 3) == 0)
        //            {
        //                healthBar.SetColor(Color.black);
        //            }
        //            else
        //            {
        //                healthBar.SetColor(Color.yellow);
        //            }
        //        }
        //    }
        //}, .03f);
    }
}
