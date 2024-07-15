using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    public float damage = 20f;
    public float radius = 1f;
    public LayerMask layerMask;

    [SerializeField] private GameObject weapon;
    private ExperienceManager experienceManager;
    private Slot weaponSlot;
    private EquipmentData weaponEquipment;

    private void Awake()
    {
        experienceManager = FindObjectOfType<ExperienceManager>();
        InitializeWeapon();
    }

    private void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);

        if (hits.Length > 0)
        {
            HandleHit(hits[0]);
        }
    }

    private void InitializeWeapon()
    {
        if (weapon != null)
        {
            weaponSlot = weapon.GetComponent<Slot>();
            if (weaponSlot != null)
            {
                weaponEquipment = weaponSlot.ItemInSlot as EquipmentData;
            }
        }
    }

    private void HandleHit(Collider hit)
    {
        PlayerStatus player = hit.GetComponentInParent<PlayerStatus>();
        Enemy enemy = hit.GetComponentInParent<Enemy>();
        Minotaur epicEnemy = hit.GetComponentInParent<Minotaur>();
        AttackManager attackManager = GetComponentInParent<AttackManager>();

        if (enemy != null)
        {
            HandleEnemyHit(enemy);
        }
        else if (epicEnemy != null)
        {
            HandleEpicEnemyHit(epicEnemy);
        }
        else if (player != null)
        {
            HandlePlayerHit(player, attackManager);
        }
    }

    private void HandleEnemyHit(Enemy enemy)
    {
        if (experienceManager.currentLevel + 5 < enemy.MonsterLevel)
        {
            enemy.Damage(1f);
        }
        else
        {
            float totalDamage = damage;
            if (weaponEquipment != null)
            {
                totalDamage += weaponEquipment.Damage;
            }
            enemy.Damage(totalDamage);
        }
        gameObject.SetActive(false);
    }

    private void HandleEpicEnemyHit(Minotaur epicEnemy)
    {
        epicEnemy.Damage(damage);
        gameObject.SetActive(false);
    }

    private void HandlePlayerHit(PlayerStatus player, AttackManager attackManager)
    {
        player.PlayerDamage(damage);

        int randomHit = Random.Range(1, 3);
        if (attackManager != null && attackManager.isNormalEnemy)
        {
            player.animator.SetTrigger("HitNormalEnemy");
        }
        else
        {
            player.animator.SetTrigger("Hit" + randomHit);
        }

        gameObject.SetActive(false);
    }
}
