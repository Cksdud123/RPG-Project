using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    [Header("Skill")]
    public SkillSO SkillObject;
    public RawImage SkillImage;
    public TextMeshProUGUI SkillName;

    [Header("Skill Info Panel")]
    public GameObject Skill_Info_Panel;
    public QuickSkillSlotPanel QuickSkillSlotPanel;

    public void SetSkill()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            SetUpdateSkill();
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    private void SetUpdateSkill()
    {
        SkillImage.texture = SkillObject.icon;
        SkillName.text = SkillObject.SkillName;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Skill_Info_Panel.GetComponent<Skill_Info>().clickedSkill = SkillObject;
        Skill_Info_Panel.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Skill_Info_Panel.SetActive(false);
    }

    public void Register()
    {
        QuickSkillSlotPanel.SkillRegistration(SkillObject);
    }
    public void Cancel()
    {
        QuickSkillSlotPanel.SkillCancel(SkillObject);
    }
}
