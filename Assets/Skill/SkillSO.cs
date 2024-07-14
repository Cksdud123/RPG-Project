using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SkillSO : ScriptableObject
{
    // 데미지, 쿨타임, 소요마나, 필요 레벨
    public float damage;
    public float cool;
    public float mana;
    public int Level;

    // 애니메이션 이름, 아이콘, 스킬이름
    [TextArea(3, 3)]
    public string description;
    public string animationName;
    public string SkillName;
    public Texture icon;
}
