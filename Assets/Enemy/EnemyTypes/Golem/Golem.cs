using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
    [SerializeField] private EnemyHealthBar healthBar;

    private Rigidbody rigid;
    void Awake()
    {
        base.Awake();
        rigid = GetComponent<Rigidbody>();
        MaxHealth = 20; // Skeleton Ŭ������ MaxHealth ����
    }
    void Start()
    {
        CurrentHealth = MaxHealth;
        healthBar.UpdateHealthBar(CurrentHealth, MaxHealth);
    }
    public override void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        healthBar.UpdateHealthBar(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0f)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Damage");
        }
    }
    public override void Die()
    {
        base.Die(); // �⺻ Ŭ���� Die �޼��� ȣ��
        rigid.isKinematic = true;
    }
}