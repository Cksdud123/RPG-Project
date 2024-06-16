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

    [Header("Status")]
    [SerializeField] private float StrikingPower;
    [SerializeField] private float DefensivePower;
    [SerializeField] private float Damage;
    [SerializeField] private float Speed;
}
