using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SkillSO : ScriptableObject
{
    // ������, ��Ÿ��, �ҿ丶��, �ʿ� ����
    public float damage;
    public float cool;
    public float mana;
    public int Level;

    // �ִϸ��̼� �̸�, ������, ��ų�̸�
    [TextArea(3, 3)]
    public string description;
    public string animationName;
    public string SkillName;
    public Texture icon;
}
