using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class QuickSkillSlotPanel : MonoBehaviour
{
    public QuickSkillSlot[] quickSkillSlot = new QuickSkillSlot[2];

    private void Awake()
    {
        for (int i = 0; i < quickSkillSlot.Length; i++)
        {
            quickSkillSlot[i].initSkillSlot();
        }
    }

    public void SkillRegistration()
    {
        for (int i = 0; i < quickSkillSlot.Length; i++)
        {
            // 1. 스킬 등록
            if(quickSkillSlot[i].skillSO != null)
            {

            }
            else
            {

            }
        }
    }
}
