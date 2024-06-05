using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    public ItemData ItemInSlot;
    public int AmountInSlot;
    [SerializeField] private DropItem dropItems;

    RawImage icon;
    TextMeshProUGUI txt_amount;

    // ���� �ʱ�ȭ
    public void initSlot()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    // ���� ����
    public void SetSlot()
    {
        // Slot�� �ڽİ�ü�� Ȱ��ȭ �� ��
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        // �ڽ� ��ü�� ������Ʈ�� ����
        icon = GetComponentInChildren<RawImage>();
        txt_amount = GetComponentInChildren<TextMeshProUGUI>();

        UpdateSlot();
    }
    // ���� ������Ʈ
    public void UpdateSlot()
    {
        if (ItemInSlot != null)
        {
            icon.texture = ItemInSlot.ITEMICON;
            txt_amount.text = $"{AmountInSlot}";
            txt_amount.enabled = true;
        }
        else
        {
            icon.texture = null;
            txt_amount.text = "0";
            txt_amount.enabled = false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DragSlot draggableItem = dropped.GetComponent<DragSlot>();
        Slot slot = draggableItem.originParent.GetComponent<Slot>();

        // ���� ������ ������ �������
        if (ItemInSlot == null)
        {
            // ����Ʈ ����� ��
            if (draggableItem.ShihtMode) HalfItemAmount(draggableItem, slot);
            // �Ϲ� ��� �϶�
            else ChangeEmptySlot(draggableItem, slot);
        }
        // ���� ������ ������ �ְ� ���̵� �ٸ���
        else if(ItemInSlot != null && ItemInSlot.ID != slot.ItemInSlot.ID) SwapItems(draggableItem, slot);
        // ���� ������ ������ �ְ� ���̵� ������
        else if(ItemInSlot != null && ItemInSlot.ID == slot.ItemInSlot.ID) AddItems(draggableItem, slot);

    }
    // �������� �� �������� ������ ����
    private void ChangeEmptySlot(DragSlot draggableItem, Slot slot)
    {
        // �Ű��� �������� �������� ���� �������� �ű�
        draggableItem.dragItemIcon.SetParent(transform);
        draggableItem.dragItemIcon.SetSiblingIndex(0);
        draggableItem.dragItemIcon.localPosition = Vector3.zero;

        // ���� ������ ������ ������ ����
        ItemInSlot = slot.ItemInSlot;
        AmountInSlot = slot.AmountInSlot;

        // ���� ������ ������ ������ �ʱ�ȭ
        slot.ItemInSlot = null;
        slot.AmountInSlot = 0;

        // ���� ������ �ڽİ�ü�� �ִ� �ؽ�Ʈ ���� �ʱ�ȭ
        slot.txt_amount.text = slot.AmountInSlot.ToString();
        slot.txt_amount.gameObject.SetActive(false);

        // RawImage�� ���� null�� ��ü�� ã�Ƽ� ����
        Transform emptyRawImage = FindEmptyRawImage(transform);
        emptyRawImage.SetParent(draggableItem.originParent);
        emptyRawImage.SetSiblingIndex(0);
        emptyRawImage.localPosition = Vector3.zero;

        SetSlot();
    }
    private void HalfItemAmount(DragSlot draggableItem, Slot slot)
    {
        // ���� ������ 1�̸� ����
        if (slot.AmountInSlot == 1) return;

        // ���� ������ ������ ������ ����
        ItemInSlot = slot.ItemInSlot;
        AmountInSlot = Mathf.CeilToInt(slot.AmountInSlot / 2f);
        SetSlot();

        // ���� ������ ������ ������ ����
        slot.AmountInSlot -= AmountInSlot;
        slot.UpdateSlot();
    }
    // �� �������� ��ü
    private void SwapItems(DragSlot draggableItem,Slot slot)
    {
        // ���� ������ ������ �����͸� �ӽ� ������ ����
        ItemData tempItem = ItemInSlot;
        int tempAmount = AmountInSlot;

        // ���� ���Կ� �巡���� ������ �����͸� �Ҵ�
        ItemInSlot = slot.ItemInSlot;
        AmountInSlot = slot.AmountInSlot;
        UpdateSlot();

        // �巡���� ���Կ� �ӽ� ����� ������ �����͸� �Ҵ�
        slot.ItemInSlot = tempItem;
        slot.AmountInSlot = tempAmount;
        slot.UpdateSlot();
    }
    // ���̵� ������ ����
    private void AddItems(DragSlot draggableItem, Slot slot)
    {
        // �ΰ��� ���Կ� �ִ� �������� ���� �ִ� �����϶�
        if(AmountInSlot == ItemInSlot.MAXSTACK && slot.AmountInSlot == slot.ItemInSlot.MAXSTACK) return;
        // �ΰ��� ���Կ� �ִ� �������� �ϳ��� �ִ� �����϶�
        if(AmountInSlot == ItemInSlot.MAXSTACK || slot.AmountInSlot == slot.ItemInSlot.MAXSTACK) SwapItems(draggableItem, slot);
        // �ΰ��� ���Կ� �ִ� �������� �������� �ִ� ������ �ѱ��� ������
        if(AmountInSlot + slot.AmountInSlot <= ItemInSlot.MAXSTACK)
        {
            // ���� ���Կ� ������ ���� ��
            AmountInSlot += slot.AmountInSlot;
            UpdateSlot();

            // ���� ������ ������ ������ �ʱ�ȭ
            slot.ItemInSlot = null;
            slot.AmountInSlot = 0;

            // ���� ������ �ڽİ�ü�� �ִ� �����͸� �ʱ�ȭ
            slot.icon.texture = null;
            slot.icon.gameObject.SetActive(false);
            slot.txt_amount.text = slot.AmountInSlot.ToString();
            slot.txt_amount.gameObject.SetActive(false);

            slot.UpdateSlot();
        }
        // �ΰ��� ���Կ� �ִ� �������� �������� �ִ� ������ �ѱ涧
        else if (AmountInSlot + slot.AmountInSlot >= ItemInSlot.MAXSTACK)
        {
            // �����ģ ������ ���� ��
            int overStack = AmountInSlot + slot.AmountInSlot;

            // ���� ������ ������ �ִ��
            AmountInSlot = ItemInSlot.MAXSTACK;
            UpdateSlot();
            // ���� ���Կ� ���� ������ ����
            slot.AmountInSlot = overStack - AmountInSlot;
            slot.UpdateSlot();
        }
    }
    // �������� ����
    public void DropItems(Slot slot)
    {
        // 0���� ����
        if(slot.AmountInSlot == 0) return;

        // ������ ���� ���� ���ð��� ���ٸ�
        if(dropItems.dropAmount == slot.AmountInSlot)
        {
            // ���� ������ ������ ������ �ʱ�ȭ
            slot.ItemInSlot = null;
            slot.AmountInSlot = 0;

            // ���� ������ �ڽİ�ü�� �ִ� �ؽ�Ʈ ���� �ʱ�ȭ
            slot.txt_amount.text = AmountInSlot.ToString();
            slot.txt_amount.gameObject.SetActive(false);
            // ���� ������ ������ �ʱ�ȭ
            slot.icon.texture = null;
            slot.icon.gameObject.SetActive(false);

            slot.UpdateSlot();
        }
        // �ִ뽺���� �ƴ϶��
        else if (dropItems.dropAmount <= slot.ItemInSlot.MAXSTACK)
        {
            // ���� ������ ������ ������ �ʱ�ȭ
            slot.AmountInSlot -= dropItems.dropAmount;

            // DropItem�� Slider �ִ� ������ ����
            dropItems.DropCountSlider.maxValue = slot.AmountInSlot;

            // ���� ������ �ڽİ�ü�� �ִ� �ؽ�Ʈ ���� �ʱ�ȭ
            slot.txt_amount.text = slot.AmountInSlot.ToString();

            slot.UpdateSlot();
        }
    }
    // �ڽİ�ü�� RawImage�� ã��
    private Transform FindEmptyRawImage(Transform parent)
    {
        foreach (Transform child in parent)
        {
            RawImage rawImage = child.GetComponent<RawImage>();
            if (rawImage != null && rawImage.texture == null)
            {
                return child;
            }
        }
        return null;
    }
}