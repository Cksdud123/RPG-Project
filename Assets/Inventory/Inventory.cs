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
    public void pickUpItem(ItemInfo itemObj)
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
                    Destroy(itemObj.gameObject);
                    slots[i].SetSlot();
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
                    pickUpItem(itemObj);
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
                Destroy(itemObj.gameObject);
                return;
            }
        }
    }
    public void SortInventory()
    {
        int SortCount = 0;

        // SortSlot이라는 변수를 선언한 뒤
        var SortSlot = slots
            // 각 slots에서 아이템이 있는 경우에만
            .Where(slot => slot.ItemInSlot != null)
            // 그 slots의 아이템 정보를 뽑아서 slot에 저장한뒤
            .Select(slot => new { slot.ItemInSlot, slot.AmountInSlot })
            // ID 순으로 정렬 후에
            .OrderBy(slot => slot.ItemInSlot.ID)
            // 리스트로 저장후 SortSlot에 반환
            .ToList();

        // 정렬할 Count를 더한 다음
        for(int i = 0; i< SortSlot.Count; i++)
        {
            SortCount += SortSlot[i].AmountInSlot;
        }
        // 정렬된 데이터를 다시 슬롯에 할당
        for (int i = 0; i < slots.Length; i++)
        {
            // 정렬된 데이터의 크기까지만 슬롯을 채움
            if (i < SortSlot.Count)
            {
                // 정렬할 SortCount의 갯수가 최대스택보다 작다면 그냥 집어넣음
                if (SortCount <= slots[i].ItemInSlot.MAXSTACK && slots[i] == null)
                {
                    slots[i].ItemInSlot = SortSlot[i].ItemInSlot;
                    slots[i].AmountInSlot = SortCount;
                }
                // 그렇지 않다면
                else
                {

                }
            }
            else
            {
                slots[i].ItemInSlot = null;
                slots[i].AmountInSlot = 0;
            }

            slots[i].SetSlot();
            if (slots[i].ItemInSlot == null) slots[i].initSlot();
        }
    }

    bool limitStack(int index, int amount)
    {
        // AmountInSlot + amount 이 값이 최대스택보다 크다면
        if (slots[index].ItemInSlot.MAXSTACK <= slots[index].AmountInSlot + amount)
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

