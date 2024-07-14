using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSkillBook : MonoBehaviour
{
    public SkillSlot[] SkillInfo = new SkillSlot[3];

    private void Awake()
    {
        for (int i = 0; i < SkillInfo.Length; i++)
        {
            SkillInfo[i].SetSkill();
        }
    }

}
