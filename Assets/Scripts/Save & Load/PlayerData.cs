using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotData
{
    public int itemDataID;
    public int amount;
    public int slotIndex;

    public SlotData(Slot slot)
    {
        // ΩΩ∑‘¿Ã ¿÷¿ª∂ß∏∏
        if (slot.ItemInSlot != null)
        {
            itemDataID = slot.ItemInSlot.ID;
            amount = slot.AmountInSlot;
            slotIndex = slot.slotIndex;
        }
        else
        {
            itemDataID = -1;
            amount = 0;
            slotIndex = slot.slotIndex;
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

    public PlayerData(PlayerStatus playerStatus, Inventory inventory)
    {
        level = playerStatus.experienceManager.currentLevel;
        money = playerStatus.PlayerMoney;
        health = playerStatus.healthBar.healthSlider.value;

        position = new float[3];
        position[0] = playerStatus.transform.position.x;
        position[1] = playerStatus.transform.position.y;
        position[2] = playerStatus.transform.position.z;

        // ƒ¸ΩΩ∑‘ µ•¿Ã≈Õ
        SaveSlotData = new SlotData[inventory.slots.Length];
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            SaveSlotData[i] = new SlotData(inventory.slots[i]);
        }
    }
}