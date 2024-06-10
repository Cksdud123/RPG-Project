using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }
    public Animator animator;

    NavMeshAgent agent;
    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        CurrentHealth = MaxHealth;
    }
    public virtual void Damage(float damageAmount)
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

    public virtual void Die()
    {
        animator.SetTrigger("Die");

        GetComponent<Collider>().enabled = false;
        agent.enabled = false;
    }
    public virtual void ItemDropEnemy() {}
}
