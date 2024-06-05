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
            // ���Կ� �������� �ְ� �������� ���̵� �����鼭 �ִ뽺���� �ƴҶ�
            if (slots[i].ItemInSlot != null && slots[i].ItemInSlot.ID == itemObj.iteminfo.ID && slots[i].ItemInSlot.MAXSTACK != slots[i].AmountInSlot)
            {
                // �ִ뽺�ÿ� �������� �ʾҴٸ�
                if (!limitStack(i, itemObj.amount))
                {
                    // �������� ������ �߰��� ��
                    slots[i].AmountInSlot += itemObj.amount;
                    Destroy(itemObj.gameObject);
                    slots[i].SetSlot();
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
                    pickUpItem(itemObj);
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
                Destroy(itemObj.gameObject);
                return;
            }
        }
    }
    public void SortInventory()
    {
        int SortCount = 0;

        // SortSlot�̶�� ������ ������ ��
        var SortSlot = slots
            // �� slots���� �������� �ִ� ��쿡��
            .Where(slot => slot.ItemInSlot != null)
            // �� slots�� ������ ������ �̾Ƽ� slot�� �����ѵ�
            .Select(slot => new { slot.ItemInSlot, slot.AmountInSlot })
            // ID ������ ���� �Ŀ�
            .OrderBy(slot => slot.ItemInSlot.ID)
            // ����Ʈ�� ������ SortSlot�� ��ȯ
            .ToList();

        // ������ Count�� ���� ����
        for(int i = 0; i< SortSlot.Count; i++)
        {
            SortCount += SortSlot[i].AmountInSlot;
        }
        // ���ĵ� �����͸� �ٽ� ���Կ� �Ҵ�
        for (int i = 0; i < slots.Length; i++)
        {
            // ���ĵ� �������� ũ������� ������ ä��
            if (i < SortSlot.Count)
            {
                // ������ SortCount�� ������ �ִ뽺�ú��� �۴ٸ� �׳� �������
                if (SortCount <= slots[i].ItemInSlot.MAXSTACK && slots[i] == null)
                {
                    slots[i].ItemInSlot = SortSlot[i].ItemInSlot;
                    slots[i].AmountInSlot = SortCount;
                }
                // �׷��� �ʴٸ�
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
        // AmountInSlot + amount �� ���� �ִ뽺�ú��� ũ�ٸ�
        if (slots[index].ItemInSlot.MAXSTACK <= slots[index].AmountInSlot + amount)
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

