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

        // ���Ե����͸� ������ ����Ʈ�� �ʱ�ȭ
        allInventoryData = new List<SlotData[]>();
        // �κ��丮 ��ũ��Ʈ�� �ִ� ��ŭ for���� ���鼭
        foreach (var inventory in playerStatus.inventories)
        {
            // ���� �迭�� �ʱ�ȭ�ϰ�
            SlotData[] slotDataArray = new SlotData[inventory.slots.Length];
            // ������ ����
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                slotDataArray[i] = new SlotData(inventory.slots[i]);
            }
            // ������ ���Ե����͸� ����Ʈ�� ����
            allInventoryData.Add(slotDataArray);
        }
    }
}