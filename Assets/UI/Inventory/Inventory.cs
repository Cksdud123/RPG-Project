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
            if (slots[i].ItemInSlot != null && slots[i].ItemInSlot.ID == itemObj.iteminfo.ID && slots[i].ItemInSlot.MAXSTACK != slots[i].AmountInSlot)
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
        // SortSlot이라는 변수를 선언한 뒤
        var SortSlot = slots
            // 각 slots에서 아이템이 있는 경우에만
            .Where(slot => slot.ItemInSlot != null)
            // 같은 아이디끼리 그룹화
            .GroupBy(slot => slot.ItemInSlot.ID)
            // 그룹 내에서 아이템의 총량을 계산
            .Select(group => new {Item = group.First().ItemInSlot, TotalAmount = group.Sum(slot => slot.AmountInSlot) })
            // ID 순으로 정렬
            .OrderBy(slot => slot.Item.ID)
            // 리스트로 저장후 SortSlot에 반환
            .ToList();

        int slotIndex = 0;

        // SortSlot에 아이템이 저장되 있는 경우에만
        foreach (var itemGroup in SortSlot)
        {
            // 아이템의 최대값을 저장한 뒤에
            int CurrentAmount = itemGroup.TotalAmount;

            // 남은 아이템이 있고, 슬롯 인덱스가 슬롯의 길이보다 작은 경우 
            while (CurrentAmount > 0 && slotIndex < slots.Length)
            {
                // 현재 아이템의 최대값이 최대스택보다 크다면
                if (CurrentAmount > itemGroup.Item.MAXSTACK)
                {
                    // 최대스택을 할당한 뒤에
                    slots[slotIndex].ItemInSlot = itemGroup.Item;
                    slots[slotIndex].AmountInSlot = itemGroup.Item.MAXSTACK;
                    // 남은양을 구한뒤에
                    CurrentAmount -= itemGroup.Item.MAXSTACK;
                }
                // 현재 아이템의 최대값이 최대스택보다 작다면
                else
                {
                    // 남은양을 슬롯에 할당한 뒤에
                    slots[slotIndex].ItemInSlot = itemGroup.Item;
                    slots[slotIndex].AmountInSlot = CurrentAmount;
                    // 현재 아이템의 값을 0으로 설정
                    CurrentAmount = 0;
                }
                // 현재 인덱스의 슬롯을 설정한 뒤
                slots[slotIndex].SetSlot();
                // 아이템이 없는 슬롯을 비활성화 
                if (slots[slotIndex].ItemInSlot == null) slots[slotIndex].initSlot();
                // 인덱스 증가
                slotIndex++;
            }
        }

        // 남은 슬롯 초기화
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