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
        MaxHealth = 20; // Skeleton 클래스의 MaxHealth 설정
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
        base.Die(); // 기본 클래스 Die 메서드 호출
        rigid.isKinematic = true;
    }
}