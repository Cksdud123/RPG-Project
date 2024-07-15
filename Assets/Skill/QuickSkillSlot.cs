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
        // Slot�� �ڽİ�ü�� Ȱ��ȭ �� ��
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

    // ��ų Ȱ��ȭ
    public void Skill()
    {
        if (skillSO != null && CoolTimeImage.fillAmount == 0 && StaminaBar.instance.stamina >= skillSO.mana)
        {
            // ��ų ����
            playerController.ActivateSkill(skillSO);
            // ��Ÿ�� Ȱ��ȭ
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
