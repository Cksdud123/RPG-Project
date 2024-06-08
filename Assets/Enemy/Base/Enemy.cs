using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }
    public Animator animator;

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }
    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;

        if(CurrentHealth <= 0f)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Damage");
        }
    }

    public void Die()
    {
        animator.SetTrigger("Die");
        GetComponent<Collider>().enabled = false;
    }
}
