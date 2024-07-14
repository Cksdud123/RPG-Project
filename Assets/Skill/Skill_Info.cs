using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Info : MonoBehaviour
{
    public SkillSO clickedSkill;
    public ExperienceManager experienceManager;

    [SerializeField] private RawImage SkillImage;
    [SerializeField] private TextMeshProUGUI SkillName;
    [SerializeField] private TextMeshProUGUI SkillLevel;
    [SerializeField] private TextMeshProUGUI SkillCoolTime;
    [SerializeField] private TextMeshProUGUI SkillStamina;
    [SerializeField] private TextMeshProUGUI SkillDesc;
    private void OnEnable()
    {
        SettingSkillInfo();
    }
    private void SettingSkillInfo()
    {
        SkillImage.texture = clickedSkill.icon;
        SkillName.text = clickedSkill.SkillName;
        SkillLevel.text = $"Lv. : {clickedSkill.Level}";
        SkillCoolTime.text = $"쿨타임 : {clickedSkill.cool.ToString()}";
        SkillStamina.text = $"소비 마나 : {clickedSkill.mana.ToString()}";
        SkillDesc.text = clickedSkill.description;

        if(experienceManager.currentLevel < clickedSkill.Level)
        {
            SkillLevel.color = Color.red;
        }
        else
        {
            SkillLevel.color = Color.yellow;
        }
    }
}
