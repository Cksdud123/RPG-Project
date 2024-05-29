using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Inventory : MonoBehaviour
{
    public Slot[] slots = new Slot[25];

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
        // 아이템 습득, 슬롯을 확인해서 슬롯에 집어넣음
        Debug.Log("아이템 습득 완료!!");
        for (int i = 0; i < slots.Length; i++)
        {
            // 슬롯에 아이템이 있고 아이템의 아이디가 같을때
            if (slots[i].ItemInSlot != null && slots[i].ItemInSlot.ID == itemObj.iteminfo.ID && slots[i].ItemInSlot.MAXSTACK != slots[i].AmountInSlot)
            {
                // 최대스택에 도달하지 않았다면
                if (!limitStack(i, itemObj.amount))
                {
                    // 아이템을 추가하고 객체를 파괴함
                    slots[i].AmountInSlot += itemObj.amount;
                    slots[i].AddSlot();
                    Destroy(itemObj.gameObject);
                    return;
                }
                // 최대 스택에 도달했으면
                else
                {
                    // 남은 양을 계산해서 재귀호출
                    int result = NeededToFill(i);
                    itemObj.amount = RemainingAmount(i, itemObj.amount);
                    slots[i].AmountInSlot += result;
                    slots[i].AddSlot();
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
                slots[i].AddSlot();
                Destroy(itemObj.gameObject);
                return;
            }
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
