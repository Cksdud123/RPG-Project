using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    public float damage = 20f;
    public float radius = 1f;
    public LayerMask layerMask;

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);

        if (hits.Length > 0)
        {
            int RandomHit = Random.Range(1, 3);

            Enemy enemy = hits[0].gameObject.GetComponentInParent<Enemy>();
            EpicEnemyHealthBar epicEnemy = hits[0].gameObject.GetComponentInParent<EpicEnemyHealthBar>();
            PlayerStatus player = hits[0].gameObject.GetComponentInParent<PlayerStatus>();

            AttackManager attackManager = GetComponentInParent<AttackManager>();
            if (enemy != null)
            {
                enemy.Damage(damage);
                gameObject.SetActive(false);
            }
            else if (epicEnemy != null)
            {
                epicEnemy.Damage(damage);
                gameObject.SetActive(false);
            }
            else if (player != null)
            {
                player.PlayerDamage(damage);
                if (attackManager != null && attackManager.isNormalEnemy)
                {
                    player.animator.SetTrigger("HitNormalEnemy");
                }
                else
                {
                    player.animator.SetTrigger("Hit" + RandomHit);
                }
                gameObject.SetActive(false);
            }
        }
    }
}
