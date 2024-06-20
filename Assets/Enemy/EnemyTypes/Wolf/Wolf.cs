using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wolf : Enemy
{
    [SerializeField] private EnemyHealthBar healthBar;

    [Header("Item Drop")]
    [SerializeField] private Transform dropItemPos;
    [SerializeField] private float nondrop;
    public WeightedRandomList<GameObject> lootTable;

    [Header("Monster Name")]
    [SerializeField] private string monster;

    private Rigidbody rigid;
    protected override void Awake()
    {
        base.Awake();
        rigid = GetComponent<Rigidbody>();
        MaxHealth = 200; 
    }
    void Start()
    {
        MonsterLevel = Random.Range(11, 16);
        CurrentHealth = MaxHealth;
        healthBar.UpdateHealthBar(CurrentHealth, MaxHealth);
        MonsterName.text = MonsterLevel.ToString() + ". " + monster;
    }
    public override void Damage(float damageAmount)
    {
        base.Damage(damageAmount);
        healthBar.UpdateHealthBar(CurrentHealth, MaxHealth);
    }
    public override void Die()
    {
        base.Die(); // �⺻ Ŭ���� Die �޼��� ȣ��
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
            Debug.Log("������ ���" + dropItem.name);
        }
        else Debug.Log("�������� ��ӵ��� �ʾҽ��ϴ�");
    }
}
