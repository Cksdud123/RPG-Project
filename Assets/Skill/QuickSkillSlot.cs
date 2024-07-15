using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class QuickSkillSlot : MonoBehaviour
{
    public SkillSO skillSO;
    public Image CoolTimeImage;
    public PlayerController playerController;

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
        UpdateSkillSlot();
    }
    public void UpdateSkillSlot()
    {
        if (skillSO != null)
        {
            Skillicon.texture = skillSO.icon;
        }
        else
        {
            Skillicon.texture = null;
        }
    }

    // 스킬 활성화
    public void Skill()
    {
        if (skillSO != null && CoolTimeImage.fillAmount == 0 && StaminaBar.instance.stamina >= skillSO.mana)
        {
            // 스킬 실행
            playerController.ActivateSkill(skillSO);
            // 쿨타임 활성화
            StartCoroutine(SC_Cool());
        }
    }

    IEnumerator SC_Cool()
    {
        float tick = 1f / skillSO.cool;
        float t = 0;

        CoolTimeImage.fillAmount = 1;

        StaminaBar.instance.UseStamina(skillSO.mana);

        while (CoolTimeImage.fillAmount > 0)
        {
            CoolTimeImage.fillAmount = Mathf.Lerp(1, 0, t);
            t += (Time.deltaTime * tick);

            yield return null;
        }
    }
}
