using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    Rigidbody rigid;

    void Awake()
    {
        base.Awake();
        MaxHealth = 50; // Skeleton 클래스의 MaxHealth 설정
        rigid = GetComponent<Rigidbody>();
    }
    void Start()
    {
        CurrentHealth = MaxHealth;
    }
    public override void Damage(float damageAmount)
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
    public override void Die()
    {
        base.Die(); // 기본 클래스 Die 메서드 호출
        rigid.isKinematic = true;
    }
}