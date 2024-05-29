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
        // ������ ����, ������ Ȯ���ؼ� ���Կ� �������
        Debug.Log("������ ���� �Ϸ�!!");
        for (int i = 0; i < slots.Length; i++)
        {
            // ���Կ� �������� �ְ� �������� ���̵� ������
            if (slots[i].ItemInSlot != null && slots[i].ItemInSlot.ID == itemObj.iteminfo.ID && slots[i].ItemInSlot.MAXSTACK != slots[i].AmountInSlot)
            {
                // �ִ뽺�ÿ� �������� �ʾҴٸ�
                if (!limitStack(i, itemObj.amount))
                {
                    // �������� �߰��ϰ� ��ü�� �ı���
                    slots[i].AmountInSlot += itemObj.amount;
                    slots[i].AddSlot();
                    Destroy(itemObj.gameObject);
                    return;
                }
                // �ִ� ���ÿ� ����������
                else
                {
                    // ���� ���� ����ؼ� ���ȣ��
                    int result = NeededToFill(i);
                    itemObj.amount = RemainingAmount(i, itemObj.amount);
                    slots[i].AmountInSlot += result;
                    slots[i].AddSlot();
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
                slots[i].AddSlot();
                Destroy(itemObj.gameObject);
                return;
            }
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
