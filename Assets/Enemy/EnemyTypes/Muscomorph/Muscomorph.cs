using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muscomorph : Enemy
{
    [SerializeField] private EnemyHealthBar healthBar;

    [Header("Item Drop")]
    [SerializeField] private Transform dropItemPos;
    [SerializeField] private float nondrop;
    public WeightedRandomList<GameObject> lootTable;

    private Rigidbody rigid;
    protected override void Awake()
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
        ItemDropEnemy();
        rigid.isKinematic = true;
    }
    public void ItemDropEnemy()
    {
        lootTable.Value = nondrop;
        GameObject dropItem = lootTable.GetRandom();
        if (dropItem != null)
        {
            Instantiate(dropItem, dropItemPos.position, Quaternion.identity);
            Debug.Log("아이템 드롭" + dropItem.name);
        }
        else Debug.Log("아이템이 드롭되지 않았습니다");
    }
}
