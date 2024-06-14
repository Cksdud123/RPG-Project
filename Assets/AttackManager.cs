using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public GameObject attack_Point;
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
