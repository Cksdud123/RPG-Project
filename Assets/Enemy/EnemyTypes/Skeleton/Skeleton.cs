using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    Rigidbody rigid;

    void Awake()
    {
        base.Awake();
        MaxHealth = 50; // Skeleton Ŭ������ MaxHealth ����
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
        base.Die(); // �⺻ Ŭ���� Die �޼��� ȣ��
        rigid.isKinematic = true;
    }
}