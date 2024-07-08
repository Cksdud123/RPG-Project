using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EpicEnemyHealthBar : MonoBehaviour
{
    public EpicHealthBar healthBar;
    [SerializeField] public float MaxHealth;
    public float CurrentHealth;
    public Animator animator;

    private Rigidbody rigid;
    private NavMeshAgent agent;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        healthBar.maxHealth = MaxHealth;
        CurrentHealth = MaxHealth;
    }
    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        healthBar.takeDamage(damageAmount);

        if (CurrentHealth <= 0f)
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
        CurrentHealth = 0;

        animator.SetTrigger("Die");

        GetComponent<Collider>().enabled = false;
        agent.enabled = false;
    }
}
