using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Helmet,
    Weapon,
    Shield,
    BodyPlate,
    Pants,
    Shoes
}
[CreateAssetMenu(fileName = "New Item", menuName = "newItem/EquipmentItem")]
[System.Serializable]
public class EquipmentData : ItemData
{
    public EquipmentType equipmentType;

    // 공격력 방어력 체력 스피드
    [Header("Status")]
    [SerializeField] public float Damage;
    [SerializeField] public float Defensive;
    [SerializeField] public float Health;
    [SerializeField] public int Rainforement;
}
