using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    public Slot[] slots = new Slot[31];
    private void Awake()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].ItemInSlot == null)
            {
                slots[i].initSlot();
            }
        }
    }

    // 아이템의 정보와 오브젝트를 파괴할지 말지 정하는 변수를 할당함
    public void pickUpItem(ItemInfo itemObj, bool destroyAfterPickup)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // 슬롯에 아이템이 있고 아이템의 아이디가 같으면서 최대스택이 아닐때
            if (slots[i].ItemInSlot != null && slots[i].ItemInSlot.ID == itemObj.iteminfo.ID 
                                            && slots[i].ItemInSlot.MAXSTACK != slots[i].AmountInSlot)
            {
                // 최대스택에 도달하지 않았다면
                if (!limitStack(i, itemObj.amount))
                {
                    // 아이템의 수량을 추가해 줌
                    slots[i].AmountInSlot += itemObj.amount;
                    slots[i].UpdateSlot();
                    if (destroyAfterPickup) Destroy(itemObj.gameObject);
                    return;
                }
                // 최대스택에 도달했다면
                else
                {
                    // 남은 양을 계산해서 재귀적으로 슬롯에 집어넣음
                    int result = NeededToFill(i);
                    itemObj.amount = RemainingAmount(i, itemObj.amount);
                    slots[i].AmountInSlot += result;
                    slots[i].SetSlot();
                    pickUpItem(itemObj, destroyAfterPickup);
                    return;
                }
            }
            // 슬롯에 아이템이 없을때
            else if(slots[i].ItemInSlot == null)
            {
                // 아이템이 없다면 아이템의 정보를 업데이트 해준뒤에 추가함
                slots[i].ItemInSlot = itemObj.iteminfo;
                slots[i].AmountInSlot = itemObj.amount;
                slots[i].SetSlot();
                if (destroyAfterPickup) Destroy(itemObj.gameObject);
                return;
            }
        }
    }
    public void SortInventory()
    {
        // 딕셔너리와 아이템 데이터를 설정
        Dictionary<int, int> itemCounts = new Dictionary<int, int>();
        List<ItemData> items = new List<ItemData>();

        // Count total amounts for each item
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].ItemInSlot != null)
            {
                // 딕셔너리에 해당 키가 존재하는지 확인해서 없다면
                if (!itemCounts.ContainsKey(slots[i].ItemInSlot.ID))
                {
                    // 딕셔너리에 추가
                    itemCounts[slots[i].ItemInSlot.ID] = 0;
                    // 리스트에 해당 아이템 추가
                    items.Add(slots[i].ItemInSlot);
                }
                itemCounts[slots[i].ItemInSlot.ID] += slots[i].AmountInSlot;
            }
        }
        // 델리게이트로 Sort 완료
        items.Sort(delegate (ItemData A, ItemData B)
        {
            if (A.ID > B.ID) return 1;
            else if (A.ID < B.ID) return -1;
            return 0;
        });

        int slotIndex = 0;

        // Distribute items back to slots
        foreach (ItemData item in items)
        {
            int totalAmount = itemCounts[item.ID];
            while (totalAmount > 0 && slotIndex < slots.Length)
            {
                if (totalAmount > item.MAXSTACK)
                {
                    slots[slotIndex].ItemInSlot = item;
                    slots[slotIndex].AmountInSlot = item.MAXSTACK;
                    totalAmount -= item.MAXSTACK;
                }
                else
                {
                    slots[slotIndex].ItemInSlot = item;
                    slots[slotIndex].AmountInSlot = totalAmount;
                    totalAmount = 0;
                }
                slots[slotIndex].SetSlot();
                slotIndex++;
            }
        }

        // Clear remaining slots
        while (slotIndex < slots.Length)
        {
            slots[slotIndex].ItemInSlot = null;
            slots[slotIndex].AmountInSlot = 0;
            slots[slotIndex].SetSlot();
            slots[slotIndex].initSlot();
            slotIndex++;
        }
    }

    bool limitStack(int index, int amount)
    {
        // AmountInSlot + amount 이 값이 최대스택보다 크다면
        if (slots[index].ItemInSlot.MAXSTACK < slots[index].AmountInSlot + amount)
            return true;
        else
            return false;
    }
    int NeededToFill(int index)
    {
        // 최대스택에서 현재 가진 양빼면 최대스택에 필요로 하는 양이 나옴
        return slots[index].ItemInSlot.MAXSTACK - slots[index].AmountInSlot;
    }
    int RemainingAmount(int index, int amount)
    {
        // 현재 가지고 있는 수량 + 픽업으로 들어온양 - 최대스택 : 채워야할 남은 양
        return (slots[index].AmountInSlot + amount) - slots[index].ItemInSlot.MAXSTACK;
    }
}