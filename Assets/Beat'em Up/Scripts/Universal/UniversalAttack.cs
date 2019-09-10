using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalAttack : MonoBehaviour
{
    public LayerMask collisionLayer;
    public float radius = 1f;
    public float damage = 2f;

    public bool isPlayer, isEnemy;

    public GameObject hitVFX_Prf;
    public AnimationDelegate animationDelegate;

    private void Update()
    {
        DetectCollision();
    }

    public void DetectCollision()
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, radius, collisionLayer);

        if (hit.Length > 0)
        {
            // universal script.in player kısmında
            if (isPlayer)
            {
                Vector3 hitVFX_Pos = hit[0].transform.position;
                hitVFX_Pos.y += 1.5f; // efekt için yerden yükseklik ayarı

                if (hit[0].transform.forward.x > 0)
                {
                    hitVFX_Pos.x += .3f;
                    animationDelegate.PunchSound();
                }
                else if (hit[0].transform.forward.x < 0)
                {
                    hitVFX_Pos.x -= .3f;
                    animationDelegate.PunchSound();
                }

                var VFXtoDestroy = Instantiate(hitVFX_Prf, hitVFX_Pos, Quaternion.identity);
                Destroy(VFXtoDestroy,.3f);

                // birden fazla animasyon olduğu için knocdown animasyonunu doğru hareketle teteikleyebilmek için
                // ilgili hareketin başına Animation Event ekledik
                // ApplyDamage (int damage, bool knockDown) fonksiyonu görüldüğü üzere damage ve knockdown işlemini
                // düzenliyor. Aşağıda diyoruz ki; eğer tag.ler tutuyorsa hem damage uygula hem de knockdown bool.unu
                // true yap, eğer tag.ler tutmuyorsa sadece damage uygula ama knockdown bool.u false olsun
                if (gameObject.CompareTag(Tags.LEFT_ARM_TAG) || gameObject.CompareTag(Tags.LEFT_LEG_TAG))
                {
                    hit[0].GetComponent<HealthScript>().ApplyDamage(damage, true);
                }
                else
                {
                    hit[0].GetComponent<HealthScript>().ApplyDamage(damage, false);
                }
            }

            if (isEnemy)
            {
                hit[0].GetComponent<HealthScript>().ApplyDamage(damage, false);
            }

            gameObject.SetActive(false);
        }
    }
}
