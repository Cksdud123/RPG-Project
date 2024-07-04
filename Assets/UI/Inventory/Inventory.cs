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

    // �������� ������ ������Ʈ�� �ı����� ���� ���ϴ� ������ �Ҵ���
    public void pickUpItem(ItemInfo itemObj, bool destroyAfterPickup)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // ���Կ� �������� �ְ� �������� ���̵� �����鼭 �ִ뽺���� �ƴҶ�
            if (slots[i].ItemInSlot != null && slots[i].ItemInSlot.ID == itemObj.iteminfo.ID 
                                            && slots[i].ItemInSlot.MAXSTACK != slots[i].AmountInSlot)
            {
                // �ִ뽺�ÿ� �������� �ʾҴٸ�
                if (!limitStack(i, itemObj.amount))
                {
                    // �������� ������ �߰��� ��
                    slots[i].AmountInSlot += itemObj.amount;
                    slots[i].UpdateSlot();
                    if (destroyAfterPickup) Destroy(itemObj.gameObject);
                    return;
                }
                // �ִ뽺�ÿ� �����ߴٸ�
                else
                {
                    // ���� ���� ����ؼ� ��������� ���Կ� �������
                    int result = NeededToFill(i);
                    itemObj.amount = RemainingAmount(i, itemObj.amount);
                    slots[i].AmountInSlot += result;
                    slots[i].SetSlot();
                    pickUpItem(itemObj, destroyAfterPickup);
                    return;
                }
            }
            // ���Կ� �������� ������
            else if(slots[i].ItemInSlot == null)
            {
                // �������� ���ٸ� �������� ������ ������Ʈ ���صڿ� �߰���
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
        // ��ųʸ��� ������ �����͸� ����
        Dictionary<int, int> itemCounts = new Dictionary<int, int>();
        List<ItemData> items = new List<ItemData>();

        // Count total amounts for each item
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].ItemInSlot != null)
            {
                // ��ųʸ��� �ش� Ű�� �����ϴ��� Ȯ���ؼ� ���ٸ�
                if (!itemCounts.ContainsKey(slots[i].ItemInSlot.ID))
                {
                    // ��ųʸ��� �߰�
                    itemCounts[slots[i].ItemInSlot.ID] = 0;
                    // ����Ʈ�� �ش� ������ �߰�
                    items.Add(slots[i].ItemInSlot);
                }
                itemCounts[slots[i].ItemInSlot.ID] += slots[i].AmountInSlot;
            }
        }
        // ��������Ʈ�� Sort �Ϸ�
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
        // AmountInSlot + amount �� ���� �ִ뽺�ú��� ũ�ٸ�
        if (slots[index].ItemInSlot.MAXSTACK < slots[index].AmountInSlot + amount)
            return true;
        else
            return false;
    }
    int NeededToFill(int index)
    {
        // �ִ뽺�ÿ��� ���� ���� �绩�� �ִ뽺�ÿ� �ʿ�� �ϴ� ���� ����
        return slots[index].ItemInSlot.MAXSTACK - slots[index].AmountInSlot;
    }
    int RemainingAmount(int index, int amount)
    {
        // ���� ������ �ִ� ���� + �Ⱦ����� ���¾� - �ִ뽺�� : ä������ ���� ��
        return (slots[index].AmountInSlot + amount) - slots[index].ItemInSlot.MAXSTACK;
    }
}