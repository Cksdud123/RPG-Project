using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float HP = 100;
    public Animator animator;
    public void TakeDamage(float damageAmount)
    {
        HP -= damageAmount;

        if(HP <= 0)
        {
            animator.SetTrigger("Die");
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            animator.SetTrigger("Damage");
        }
    }
}
