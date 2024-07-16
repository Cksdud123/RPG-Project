using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class QuickSkillSlotPanel : MonoBehaviour
{
    public QuickSkillSlot[] quickSkillSlot = new QuickSkillSlot[2];
    public PlayerController controller;
    public AttackManager attackManager;

    [HideInInspector]public bool isUsed = false;
    private void Awake()
    {
        for (int i = 0; i < quickSkillSlot.Length; i++)
        {
            quickSkillSlot[i].initSkillSlot();
        }
    }
    private void Update()
    {
        UseSkill();
    }
    private void UseSkill()
    {
        if (isUsed || !controller.isEquipped) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateSkillSlot(0); // 1번 키는 슬롯 인덱스 0에 해당
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateSkillSlot(1); // 2번 키는 슬롯 인덱스 1에 해당
        }
    }
    public void SkillRegistration(SkillSO skill)
    {
        for (int i = 0; i < quickSkillSlot.Length; i++)
        {
            if(quickSkillSlot[i].skillSO == null)
            {
                // 비어있다면
                quickSkillSlot[i].skillSO = skill;
                quickSkillSlot[i].SetSkillSlot();
                return;
            }
        }
    }
    public void SkillCancel(SkillSO skill)
    {
        for (int i = 0; i < quickSkillSlot.Length; i++)
        {
            if (quickSkillSlot[i].skillSO == skill)
            {
                if (quickSkillSlot[i].CoolTimeImage.fillAmount != 0) return;

                quickSkillSlot[i].skillSO = null;
                quickSkillSlot[i].initSkillSlot();
                return;
            }
        }
    }
    private void ActivateSkillSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < quickSkillSlot.Length && !isUsed)
        {
            isUsed = true;
            quickSkillSlot[slotIndex].Skill();
            attackManager.skill = quickSkillSlot[slotIndex].skillSO;
            StartCoroutine(ResetIsUsedAfterSkill(controller.playerAnim.GetCurrentAnimatorStateInfo(0).length));
        }
    }

    private IEnumerator ResetIsUsedAfterSkill(float duration)
    {
        yield return new WaitForSeconds(duration);
        isUsed = false;
    }
}
