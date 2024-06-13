using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EpicBoss : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void EquitAxe()
    {
        animator.SetTrigger("EquipmentEnemy");
        EpicBossSountManager.Instance.Enemy_Scream();
    }
}
