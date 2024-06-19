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
            if (slots[i].ItemInSlot != null && slots[i].ItemInSlot.ID == itemObj.iteminfo.ID && slots[i].ItemInSlot.MAXSTACK != slots[i].AmountInSlot)
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
        // SortSlot�̶�� ������ ������ ��
        var SortSlot = slots
            // �� slots���� �������� �ִ� ��쿡��
            .Where(slot => slot.ItemInSlot != null)
            // ���� ���̵𳢸� �׷�ȭ
            .GroupBy(slot => slot.ItemInSlot.ID)
            // �׷� ������ �������� �ѷ��� ���
            .Select(group => new {Item = group.First().ItemInSlot, TotalAmount = group.Sum(slot => slot.AmountInSlot) })
            // ID ������ ����
            .OrderBy(slot => slot.Item.ID)
            // ����Ʈ�� ������ SortSlot�� ��ȯ
            .ToList();

        int slotIndex = 0;

        // SortSlot�� �������� ����� �ִ� ��쿡��
        foreach (var itemGroup in SortSlot)
        {
            // �������� �ִ밪�� ������ �ڿ�
            int CurrentAmount = itemGroup.TotalAmount;

            // ���� �������� �ְ�, ���� �ε����� ������ ���̺��� ���� ��� 
            while (CurrentAmount > 0 && slotIndex < slots.Length)
            {
                // ���� �������� �ִ밪�� �ִ뽺�ú��� ũ�ٸ�
                if (CurrentAmount > itemGroup.Item.MAXSTACK)
                {
                    // �ִ뽺���� �Ҵ��� �ڿ�
                    slots[slotIndex].ItemInSlot = itemGroup.Item;
                    slots[slotIndex].AmountInSlot = itemGroup.Item.MAXSTACK;
                    // �������� ���ѵڿ�
                    CurrentAmount -= itemGroup.Item.MAXSTACK;
                }
                // ���� �������� �ִ밪�� �ִ뽺�ú��� �۴ٸ�
                else
                {
                    // �������� ���Կ� �Ҵ��� �ڿ�
                    slots[slotIndex].ItemInSlot = itemGroup.Item;
                    slots[slotIndex].AmountInSlot = CurrentAmount;
                    // ���� �������� ���� 0���� ����
                    CurrentAmount = 0;
                }
                // ���� �ε����� ������ ������ ��
                slots[slotIndex].SetSlot();
                // �������� ���� ������ ��Ȱ��ȭ 
                if (slots[slotIndex].ItemInSlot == null) slots[slotIndex].initSlot();
                // �ε��� ����
                slotIndex++;
            }
        }

        // ���� ���� �ʱ�ȭ
        while (slotIndex < slots.Length)
        {
            slots[slotIndex].ItemInSlot = null;
            slots[slotIndex].AmountInSlot = 0;
            slots[slotIndex].SetSlot();

            if (slots[slotIndex].ItemInSlot == null) slots[slotIndex].initSlot();
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