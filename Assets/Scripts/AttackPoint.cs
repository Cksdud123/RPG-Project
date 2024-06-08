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
            Enemy enemy = hits[0].gameObject.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.Damage(damage);
                gameObject.SetActive(false);
            }
        }

    }
}
