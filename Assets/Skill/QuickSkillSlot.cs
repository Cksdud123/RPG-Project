using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSkillSlot : MonoBehaviour
{
    public SkillSO skillSO;

    [HideInInspector] public RawImage Skillicon;
    public void initSkillSlot()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void SetSkillSlot()
    {
        // Slot의 자식객체를 활성화 한 뒤
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        Skillicon = GetComponentInChildren<RawImage>();
    }
}
