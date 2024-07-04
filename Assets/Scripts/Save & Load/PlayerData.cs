using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotData
{
    public int itemDataID;
    public int amount;

    public SlotData(Slot slot)
    {
        if (slot.ItemInSlot != null)
        {
            itemDataID = slot.ItemInSlot.ID;
            amount = slot.AmountInSlot;
        }
        else
        {
            itemDataID = -1;
            amount = 0;
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public int level;
    public int money;
    public float health;
    public float[] position;

    public SlotData[] SaveSlotData;

    public List<SlotData[]> allInventoryData;
    public PlayerData(PlayerStatus playerStatus)
    {
        level = playerStatus.experienceManager.currentLevel;
        money = playerStatus.PlayerMoney;
        health = playerStatus.healthBar.healthSlider.value;

        position = new float[3];
        position[0] = playerStatus.transform.position.x;
        position[1] = playerStatus.transform.position.y;
        position[2] = playerStatus.transform.position.z;

        // 슬롯데이터를 저장할 리스트를 초기화
        allInventoryData = new List<SlotData[]>();
        // 인벤토리 스크립트가 있는 만큼 for문을 돌면서
        foreach (var inventory in playerStatus.inventories)
        {
            // 슬롯 배열을 초기화하고
            SlotData[] slotDataArray = new SlotData[inventory.slots.Length];
            // 슬롯을 저장
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                slotDataArray[i] = new SlotData(inventory.slots[i]);
            }
            // 저장한 슬롯데이터를 리스트에 저장
            allInventoryData.Add(slotDataArray);
        }
    }
}