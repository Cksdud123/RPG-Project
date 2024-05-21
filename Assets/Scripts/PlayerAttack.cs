using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Animator animator;
    int hashAttackCount = Animator.StringToHash("AttackCount");
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out animator);
        animator = GetComponent<Animator>();
    }
    public int AttackCount
    {
        get => animator.GetInteger(hashAttackCount);
        set => animator.SetInteger(hashAttackCount, value);
    }
}
