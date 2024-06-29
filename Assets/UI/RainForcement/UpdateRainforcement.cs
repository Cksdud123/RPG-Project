using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpdateRainforcement : MonoBehaviour
{
    Slot slot;

    [Header("Item Info")]
    [SerializeField] TextMeshProUGUI ItemNameTxt;
    [SerializeField] TextMeshProUGUI SucRateTxt;
    [SerializeField] TextMeshProUGUI FailRateTxt;
    [SerializeField] TextMeshProUGUI RainforceCost;
    [SerializeField] GameObject EquipGoldPanel;
    [Header("CSV")]
    [SerializeField] RainforcementTable RainforcementTable;
    [Header("PlayerStatus")]
    [SerializeField] PlayerStatus playerStatus;

    private int[] itemUpgrade;
    private int[] successful;
    private int[] fail;
    private int[] Damage;
    private int[] Defensive;
    private int[] Health;
    private int[] Gold;

    private int currentLevel;

    EquipmentData clickEquipSlot;

    private void Awake()
    {
        slot = GetComponentInChildren<Slot>();
    }
    private void Start()
    {
        int count = RainforcementTable.Rainforcement.Count;

        itemUpgrade = new int[count];
        successful = new int[count];
        fail = new int[count];
        Damage = new int[count];
        Defensive = new int[count];
        Health = new int[count];
        Gold = new int[count];

        for (int i = 0; i < count; i++)
        {
            itemUpgrade[i] = RainforcementTable.Rainforcement[i].upgrade;
            successful[i] = RainforcementTable.Rainforcement[i].successful;
            fail[i] = RainforcementTable.Rainforcement[i].fail;
            Damage[i] = RainforcementTable.Rainforcement[i].Damage;
            Defensive[i] = RainforcementTable.Rainforcement[i].Defensive;
            Health[i] = RainforcementTable.Rainforcement[i].Health;
            Gold[i] = RainforcementTable.Rainforcement[i].Gold;
        }

        SetRainforcement();
    }
    private void Update()
    {
        if(slot != null) SetRainforcement();
    }
    public void SetRainforcement()
    {
        if (slot.ItemInSlot == null)
        {
            ItemNameTxt.text = "아이템 없음";
            SucRateTxt.text = "-";
            FailRateTxt.text = "-";
            RainforceCost.text = "-";
            return;
        }
        else
        {
            clickEquipSlot = slot.ItemInSlot as EquipmentData;
            ItemNameTxt.text = slot.ItemInSlot.Name + $"+{clickEquipSlot.Rainforement}강";

            currentLevel = clickEquipSlot.Rainforement;

            if (currentLevel >= 0 && currentLevel < RainforcementTable.Rainforcement.Count)
            {
                SucRateTxt.text = $"성공확률 : {successful[currentLevel]}%";
                FailRateTxt.text = $"실패확률 : {fail[currentLevel]}%";
                RainforceCost.text = $"소요골드 : {Gold[currentLevel]}";
            }
        }
    }
    public void EnhanceEquipment()
    {
        int probability = Random.Range(0, 100);

        // 돈이 없다면 return;
        if (playerStatus.PlayerMoney < Gold[currentLevel])
        {
            EquipGoldPanel.gameObject.SetActive(true);
            return;
        }

        if (probability < successful[currentLevel])
        {
            playerStatus.PlayerMoney -= Gold[currentLevel];
            clickEquipSlot.Rainforement += 1;
            clickEquipSlot.Damage += Damage[currentLevel];
            clickEquipSlot.Defensive += Defensive[currentLevel];
            clickEquipSlot.Health += Health[currentLevel];
            SetRainforcement();
        }
        else
        {
            playerStatus.PlayerMoney -= Gold[currentLevel];
        }
    }
    public void CheckButton()
    {
        EquipGoldPanel.gameObject.SetActive(false);
    }
}
