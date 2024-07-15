using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public GameObject attack_Point;
    public SkillSO skill;
    public AttackPoint attackPoint;
    [HideInInspector] public bool isNormalEnemy;
    void Turn_On_AttackPoint()
    {
        attack_Point.SetActive(true);
    }

    void Turn_Off_AttackPoint()
    {
        if (attack_Point.activeInHierarchy)
        {
            attack_Point.SetActive(false);
        }
    }
    void Turn_On_AttackPointSkill()
    {
        attack_Point.SetActive(true);
        attackPoint.damage = skill.damage;
    }

    void Turn_Off_AttackPointSkill()
    {
        if (attack_Point.activeInHierarchy)
        {
            attack_Point.SetActive(false);
            attackPoint.damage = 20f;
        }
    }

    void Normal_Turn_On_AttackPoint()
    {
        isNormalEnemy = true;
        attack_Point.SetActive(true);
    }

    void Normal_Turn_Off_AttackPoint()
    {
        if (attack_Point.activeInHierarchy)
        {
            isNormalEnemy = false;
            attack_Point.SetActive(false);
        }
    }
}
