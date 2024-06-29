using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveEquipToolTip : MonoBehaviour
{
    public Slot clickedSlot;

    public Color[] rarityColors;

    [SerializeField] RawImage image;

    [Header("Normal Info")]
    [SerializeField] TextMeshProUGUI txt_Name;
    [SerializeField] TextMeshProUGUI txt_Rarity;
    [SerializeField] TextMeshProUGUI txt_Weight;
    [SerializeField] TextMeshProUGUI txt_Price;
    [SerializeField] TextMeshProUGUI txt_Type;
    [SerializeField] TextMeshProUGUI txt_Durability;

    [Header("Equip Info")]
    [SerializeField] TextMeshProUGUI txt_Damage;
    [SerializeField] TextMeshProUGUI txt_PlusHealth;
    [SerializeField] TextMeshProUGUI txt_Defensive;

    [SerializeField] TextMeshProUGUI txt_Description;

    private void OnEnable()
    {
        SettingToolTip();
    }
    private void SettingToolTip()
    {
        EquipmentData clickEquipSlot = clickedSlot.ItemInSlot as EquipmentData;

        txt_Name.text = clickedSlot.ItemInSlot.Name + $"+{clickEquipSlot.Rainforement}��";
        txt_Weight.text = $"���� : {clickedSlot.ItemInSlot.Weight * clickedSlot.AmountInSlot}kg";
        txt_Price.text = $"���� : {clickedSlot.ItemInSlot.Price}��";
        txt_Durability.text = $"������ : {clickedSlot.ItemInSlot.Durability}";

        txt_Damage.text = $"������ : {clickEquipSlot.Damage.ToString()}";
        txt_PlusHealth.text = $"�߰�ü�� : {clickEquipSlot.Health.ToString()}";
        txt_Defensive.text = $"���� : {clickEquipSlot.Defensive.ToString()}";
        txt_Description.text = clickEquipSlot.Description;

        txt_Rarity.text = clickedSlot.ItemInSlot.RARITY.ToString();
        txt_Type.text = clickedSlot.ItemInSlot.ITEMTYPE.ToString();

        if (clickedSlot.ItemInSlot.RARITY == Rarity.Common)
        {
            image.color = rarityColors[0];
        }
        if (clickedSlot.ItemInSlot.RARITY == Rarity.Uncommon)
        {
            image.color = rarityColors[1];
        }
        if (clickedSlot.ItemInSlot.RARITY == Rarity.Rare)
        {
            image.color = rarityColors[2];
        }
        if (clickedSlot.ItemInSlot.RARITY == Rarity.Epic)
        {
            image.color = rarityColors[3];
        }
        if (clickedSlot.ItemInSlot.RARITY == Rarity.Legendary)
        {
            image.color = rarityColors[4];
        }
    }
}
