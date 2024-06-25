using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Enemy : Poolable, IDamageable
{
    [SerializeField] public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public int MonsterLevel { get; set; }
    public TextMeshProUGUI MonsterName { get; set; }

    public Animator animator;

    private ExperienceManager levelManaged;
    private EnemySpawner spawner;
    private EnemyHealthBar enemyhealthBar;
    private PlayerStatus playerStatus;

    NavMeshAgent agent;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        levelManaged = FindObjectOfType<ExperienceManager>();
        enemyhealthBar = GetComponentInChildren<EnemyHealthBar>();
        MonsterName = GetComponentInChildren<TextMeshProUGUI>();
        playerStatus = FindObjectOfType<PlayerStatus>();
        spawner = FindObjectOfType<EnemySpawner>();
    }

    protected virtual void Start()
    {
        CurrentHealth = MaxHealth;
    }
    private void Update()
    {
        // �÷��̾��� ���� �������� ������ ������ ���� ��� ���� ����
        if (MonsterLevel > levelManaged.currentLevel + 5) MonsterName.color = Color.red;
        else MonsterName.color = Color.white; // �⺻ ����
    }
    public void InitializeHealth()
    {
        CurrentHealth = MaxHealth;
        if (enemyhealthBar != null)
        {
            enemyhealthBar.ResetHealthBar(); // ü�� �� �ʱ�ȭ
            enemyhealthBar.UpdateHealthBar(CurrentHealth, MaxHealth); // ü�� �� ������Ʈ
        }
    }
    public virtual void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), Random.Range(0.5f, 0.75f), Random.Range(0f, 0.25f));
        DamagePopUpGenerator.current.CreatePopUp(transform.position + randomness, damageAmount.ToString(), Color.yellow);

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
        CurrentHealth = 0;

        levelManaged.AddExperience(Random.Range(5, 15));
        playerStatus.PlayerMoney += Random.Range(5, 15);
        animator.SetTrigger("Die");

        GetComponent<Collider>().enabled = false;
        agent.enabled = false;

        StartCoroutine(Respawner(3f)); 
    }
    public IEnumerator Respawner(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        spawner.DecrementEnemyCount();
        ReleaseObject();
    }
    public void SetSpawner(EnemySpawner spawner)
    {
        this.spawner = spawner;
    }
}
