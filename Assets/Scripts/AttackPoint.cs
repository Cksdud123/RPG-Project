using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    public float damage = 20f;
    public float radius = 1f;
    public LayerMask layerMask;

    [SerializeField] private GameObject Weapon;
    ExperienceManager experienceManager;
    Slot WeaponSlot;
    EquipmentData WeaponEquipment;

    private void Awake()
    {
        experienceManager = FindObjectOfType<ExperienceManager>();
    }
    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);

        if (hits.Length > 0)
        {
            // 플레이어 데미지 레이어 랜덤 재생
            int RandomHit = Random.Range(1, 3);
            PlayerStatus player = hits[0].gameObject.GetComponentInParent<PlayerStatus>();

            Enemy enemy = hits[0].gameObject.GetComponentInParent<Enemy>();
            EpicEnemyHealthBar epicEnemy = hits[0].gameObject.GetComponentInParent<EpicEnemyHealthBar>();
            AttackManager attackManager = GetComponentInParent<AttackManager>();

            if (enemy != null)
            {
                float damageToDeal;
                if (experienceManager.currentLevel + 5 < enemy.MonsterLevel)
                {
                    damageToDeal = 1f;
                    enemy.Damage(damageToDeal);
                    gameObject.SetActive(false);
                }
                else
                {
                    ChangeDamage();
                    if (WeaponSlot.ItemInSlot != null)
                    {
                        enemy.Damage(damage + WeaponEquipment.Damage);
                    }
                    else
                    {
                        enemy.Damage(damage);
                    }
                    gameObject.SetActive(false);
                }
            }
            else if (epicEnemy != null)
            {
                epicEnemy.Damage(damage);
                gameObject.SetActive(false);
            }
            else if (player != null)
            {
                PlayerController playerController = player.GetComponentInParent<PlayerController>();
                if (playerController.isBlocking) return;

                player.PlayerDamage(damage);
                if (attackManager != null && attackManager.isNormalEnemy)
                {
                    player.animator.SetTrigger("HitNormalEnemy");
                }
                else
                {
                    player.animator.SetTrigger("Hit" + RandomHit);
                }
                gameObject.SetActive(false);
            }
        }
    }
    public void ChangeDamage()
    {
        if (Weapon != null)
        {
            WeaponSlot = Weapon.GetComponent<Slot>();
            if (WeaponSlot != null)
            {
                WeaponEquipment = WeaponSlot.ItemInSlot as EquipmentData;
            }
        }
    }
}
