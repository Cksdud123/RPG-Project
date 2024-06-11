using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Enemy : Poolable, IDamageable
{
    [SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }
    public Animator animator;

    private ExperienceManager levelManaged;
    private EnemySpawner spawner;
    private EnemyHealthBar enemyhealthBar;

    NavMeshAgent agent;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        levelManaged = FindObjectOfType<ExperienceManager>();
        enemyhealthBar = GetComponentInChildren<EnemyHealthBar>();
        spawner = FindObjectOfType<EnemySpawner>();
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }
    public void InitializeHealth()
    {
        CurrentHealth = MaxHealth;
        if (enemyhealthBar != null)
        {
            enemyhealthBar.ResetHealthBar(); // 체력 바 초기화
            enemyhealthBar.UpdateHealthBar(CurrentHealth, MaxHealth); // 체력 바 업데이트
        }
    }
    public virtual void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;

        if (CurrentHealth <= 0f)
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
        spawner.DecrementEnemyCount();

        CurrentHealth = 0;

        levelManaged.AddExperience(Random.Range(10, 50));
        animator.SetTrigger("Die");

        GetComponent<Collider>().enabled = false;
        agent.enabled = false;

        StartCoroutine(Respawner(3f)); 
    }
    public IEnumerator Respawner(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); 
        ReleaseObject();
    }
    public void SetSpawner(EnemySpawner spawner)
    {
        this.spawner = spawner;
    }

    public virtual void ItemDropEnemy() { }
}
